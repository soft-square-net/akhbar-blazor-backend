using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSH.Framework.Core.Storage.File.Features;
public record FileDownloadResponse(
    Stream fileStream,
    string contentType, 
    long contentLength,
    string contentLanguage,
    string contentDisposition,
    string contentEncoding,
    string contentMD5,
    DateTime? expires
    );
