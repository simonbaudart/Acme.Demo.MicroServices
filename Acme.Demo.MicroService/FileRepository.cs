// -----------------------------------------------------------------------
//  <copyright file="FileRepository.cs" company="Acme">
//  Copyright (c) Acme. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace Acme.Demo.MicroService
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;

    using Microsoft.Extensions.Configuration;

    public class FileRepository
    {
        private readonly IConfiguration configuration;

        public FileRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void SaveFile(string fileName, byte[] data)
        {
            var azureStorageConnectionString = this.configuration["StorageConnectionStrings:Azure"];
            var containerClient = new Azure.Storage.Blobs.BlobContainerClient(new Uri(azureStorageConnectionString));
            var blobClient = containerClient.GetBlobClient(fileName);
            blobClient.Upload(new BinaryData(data));
        }

        public void SaveBitmap(string fileName, Bitmap bitmap)
        {
            using var memoryStream = new MemoryStream();
            bitmap.Save(memoryStream, ImageFormat.Bmp);

            this.SaveFile(fileName + ".bmp", memoryStream.ToArray());
        }
    }
}