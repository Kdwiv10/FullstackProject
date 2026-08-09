using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FullstackProject.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FullstackProject.Data
{
    public static class DatabaseSeeder
    {
        public static async Task SeedSqlScriptAsync(WebApplication app)
        {
            var logger = app.Services.GetRequiredService<ILoggerFactory>().CreateLogger("DatabaseSeeder");
            try
            {
                using var scope = app.Services.CreateScope();
                var context = scope.ServiceProvider.GetService<S22024Group2ProjectContext>();
                if (context == null)
                {
                    logger.LogWarning("S22024Group2ProjectContext not available; skipping SQL seeding.");
                    return;
                }

                var contentRoot = app.Environment.ContentRootPath;
                var scriptPath = Path.Combine(contentRoot, "..", "script.sql");
                if (!File.Exists(scriptPath))
                {
                    logger.LogInformation("No script.sql found at {path}; skipping.", scriptPath);
                    return;
                }
                var script = await ReadAllTextWithEncodingAsync(scriptPath);
                // remove CREATE DATABASE blocks and USE statements to avoid permission/existence issues
                script = Regex.Replace(script, @"CREATE\s+DATABASE[\s\S]*?GO", string.Empty, RegexOptions.IgnoreCase);
                script = Regex.Replace(script, @"USE\s+\[[^\]]+\]\s*\r?\n?", string.Empty, RegexOptions.IgnoreCase);
                var batches = Regex.Split(script, @"^\s*GO\s*$(?:\r\n?|\n)?", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                foreach (var batch in batches)
                {
                    var sql = batch.Trim();
                    if (string.IsNullOrWhiteSpace(sql))
                        continue;
                    logger.LogInformation("Executing SQL batch ({length} chars)...", sql.Length);
                    try
                    {
                        await context.Database.ExecuteSqlRawAsync(sql);
                    }
                    catch (Exception ex)
                    {
                        logger.LogWarning(ex, "Ignored error executing batch (may already exist): {msg}", ex.Message);
                    }
                }

                // run optional patch.sql in repo root to fix small schema mismatches
                var patchPath = Path.Combine(contentRoot, "..", "patch.sql");
                if (File.Exists(patchPath))
                {
                    logger.LogInformation("Applying patch file at {path}", patchPath);
                        var patch = await ReadAllTextWithEncodingAsync(patchPath);
                    var patchBatches = Regex.Split(patch, @"^\s*GO\s*$(?:\r\n?|\n)?", RegexOptions.Multiline | RegexOptions.IgnoreCase);
                    foreach (var batch in patchBatches)
                    {
                        var sql = batch.Trim();
                        if (string.IsNullOrWhiteSpace(sql))
                            continue;
                        logger.LogInformation("Executing patch SQL batch ({length} chars)...", sql.Length);
                        try
                        {
                            await context.Database.ExecuteSqlRawAsync(sql);
                        }
                        catch (Exception ex)
                        {
                            logger.LogWarning(ex, "Ignored error executing patch batch: {msg}", ex.Message);
                        }
                    }
                }

                logger.LogInformation("SQL script executed successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error executing SQL script.");
            }
        }

        // Helper to detect BOM and read with correct encoding (UTF-16 LE/BE or UTF-8)
        static async Task<string> ReadAllTextWithEncodingAsync(string path)
        {
            byte[] bom = new byte[4];
            using (var fs = File.OpenRead(path))
            {
                await fs.ReadAsync(bom, 0, bom.Length);
            }
            // UTF-8 BOM: EF BB BF
            if (bom[0] == 0xEF && bom[1] == 0xBB && bom[2] == 0xBF)
                return await File.ReadAllTextAsync(path, Encoding.UTF8);
            // UTF-16 LE BOM: FF FE
            if (bom[0] == 0xFF && bom[1] == 0xFE)
                return await File.ReadAllTextAsync(path, Encoding.Unicode);
            // UTF-16 BE BOM: FE FF
            if (bom[0] == 0xFE && bom[1] == 0xFF)
                return await File.ReadAllTextAsync(path, Encoding.BigEndianUnicode);
            // Default to UTF8
            return await File.ReadAllTextAsync(path, Encoding.UTF8);
        }
    }
}
