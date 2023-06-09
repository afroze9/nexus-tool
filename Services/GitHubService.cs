﻿using System.IO.Compression;

namespace Nexus.Services;

public class GitHubService
{
    private const string ServiceTemplateUrl = @"https://codeload.github.com/nexus-framework/nexus-template/zip/refs/heads/master";
    private const string SolutionTemplateUrl = @"https://codeload.github.com/nexus-framework/nexus/zip/refs/heads/master";
    private const string LibrariesUrl = @"https://codeload.github.com/nexus-framework/nexus-libraries/zip/refs/heads/master";
    
    public async Task DownloadServiceTemplate(string destPath)
    {
        // create temp folder
        string? tempFolderPath = Path.Combine(Path.GetTempPath(), "nexus", Guid.NewGuid().ToString());
        string? downloadFilePath = Path.Combine(tempFolderPath, "template.zip");
        string? extractPath = Path.Combine(tempFolderPath, "template");
        string? templateSourcePath = Path.Combine(extractPath, "nexus-template-master", "ServiceTemplate");

        try
        {
            if (!Directory.Exists(tempFolderPath))
            {
                Directory.CreateDirectory(tempFolderPath);
            }

            // download files to temp
            Console.WriteLine("Downloading service template");
            using (HttpClient? client = new ())
            {
                HttpResponseMessage? response = await client.GetAsync(ServiceTemplateUrl);

                if (!response.IsSuccessStatusCode)
                {
                    return;
                }

                await using (Stream? contentStream = await response.Content.ReadAsStreamAsync())
                {
                    await using (FileStream? fileStream = new (downloadFilePath, FileMode.Create,
                                     FileAccess.ReadWrite,
                                     FileShare.ReadWrite))
                    {
                        await contentStream.CopyToAsync(fileStream);
                    }
                }
            }
            Console.WriteLine("Download complete");

            // Extract files
            Console.WriteLine("Extracting solution");
            if (File.Exists(downloadFilePath))
            {
                if (!Directory.Exists(extractPath))
                {
                    Directory.CreateDirectory(extractPath);
                }

                ZipFile.ExtractToDirectory(downloadFilePath, extractPath);
            }

            // move files to dest
            CopyDirectory(templateSourcePath, destPath, true);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        finally
        {
            if (Directory.Exists(tempFolderPath))
            {
                Directory.Delete(tempFolderPath, true);
            }
        }
    }
    
    public async Task DownloadSolutionTemplate(string solutionName, string solutionDirectory)
    {
        // create temp folder
        string? tempFolderPath = Path.Combine(Path.GetTempPath(), "nexus", Guid.NewGuid().ToString());
        string? downloadFilePath = Path.Combine(tempFolderPath, "template.zip");
        string? extractPath = Path.Combine(tempFolderPath, "template");
        string? templateSourcePath = Path.Combine(extractPath, "nexus-master");

        try
        {
            if (!Directory.Exists(tempFolderPath))
            {
                Directory.CreateDirectory(tempFolderPath);
            }

            // download files to temp
            Console.WriteLine("Downloading solution template");
            using (HttpClient? client = new ())
            {
                HttpResponseMessage? response = await client.GetAsync(SolutionTemplateUrl);

                if (!response.IsSuccessStatusCode)
                {
                    return;
                }

                await using (Stream? contentStream = await response.Content.ReadAsStreamAsync())
                {
                    await using (FileStream? fileStream = new (downloadFilePath, FileMode.Create,
                                     FileAccess.ReadWrite,
                                     FileShare.ReadWrite))
                    {
                        await contentStream.CopyToAsync(fileStream);
                    }
                }
            }
            Console.WriteLine("Download complete");

            // Extract files
            Console.WriteLine("Extracting solution");
            if (File.Exists(downloadFilePath))
            {
                if (!Directory.Exists(extractPath))
                {
                    Directory.CreateDirectory(extractPath);
                }

                ZipFile.ExtractToDirectory(downloadFilePath, extractPath);
            }

            // move files to dest
            CopyDirectory(templateSourcePath, solutionDirectory, true);

            Console.WriteLine("Updating config");
            string solutionFileSourcePath = Path.Combine(solutionDirectory, "nexus.sln");
            string solutionFileDestPath = Path.Combine(solutionDirectory, $"{solutionName}.sln");
            File.Move(solutionFileSourcePath, solutionFileDestPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        finally
        {
            if (Directory.Exists(tempFolderPath))
            {
                Directory.Delete(tempFolderPath, true);
            }
        }
    }
    
    private static void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
    {
        // Get information about the source directory
        DirectoryInfo? dir = new (sourceDir);

        // Check if the source directory exists
        if (!dir.Exists)
            throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

        // Cache directories before we start copying
        DirectoryInfo[] dirs = dir.GetDirectories();

        // Create the destination directory
        Directory.CreateDirectory(destinationDir);

        // Get the files in the source directory and copy to the destination directory
        foreach (FileInfo file in dir.GetFiles())
        {
            string targetFilePath = Path.Combine(destinationDir, file.Name);
            file.CopyTo(targetFilePath);
        }

        // If recursive and copying subdirectories, recursively call this method
        if (recursive)
        {
            foreach (DirectoryInfo subDir in dirs)
            {
                string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                CopyDirectory(subDir.FullName, newDestinationDir, true);
            }
        }
    }

    public async Task DownloadLibraries(string destPath)
    {
        // create temp folder
        string? tempFolderPath = Path.Combine(Path.GetTempPath(), "nexus", Guid.NewGuid().ToString());
        string? downloadFilePath = Path.Combine(tempFolderPath, "libraries.zip");
        string? extractPath = Path.Combine(tempFolderPath, "libraries");
        string? templateSourcePath = Path.Combine(extractPath, "nexus-libraries-master");

        try
        {
            if (!Directory.Exists(tempFolderPath))
            {
                Directory.CreateDirectory(tempFolderPath);
            }

            // download files to temp
            Console.WriteLine("Downloading libraries");
            using (HttpClient? client = new ())
            {
                HttpResponseMessage? response = await client.GetAsync(LibrariesUrl);

                if (!response.IsSuccessStatusCode)
                {
                    return;
                }

                await using (Stream? contentStream = await response.Content.ReadAsStreamAsync())
                {
                    await using (FileStream? fileStream = new (downloadFilePath, FileMode.Create,
                                     FileAccess.ReadWrite,
                                     FileShare.ReadWrite))
                    {
                        await contentStream.CopyToAsync(fileStream);
                    }
                }
            }
            Console.WriteLine("Download complete");

            // Extract files
            Console.WriteLine("Extracting libraries");
            if (File.Exists(downloadFilePath))
            {
                if (!Directory.Exists(extractPath))
                {
                    Directory.CreateDirectory(extractPath);
                }

                ZipFile.ExtractToDirectory(downloadFilePath, extractPath);
            }

            // move files to dest
            CopyDirectory(templateSourcePath, destPath, true);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
        finally
        {
            if (Directory.Exists(tempFolderPath))
            {
                Directory.Delete(tempFolderPath, true);
            }
        }
    }
}