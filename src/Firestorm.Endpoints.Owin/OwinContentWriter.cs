﻿using System.Threading.Tasks;
using Firestorm.Endpoints.Formatting;
using Microsoft.Owin;

namespace Firestorm.Endpoints.Owin
{
    public class OwinContentWriter : IContentWriter
    {
        private readonly IOwinContext _owinContext;

        public OwinContentWriter(IOwinContext owinContext)
        {
            _owinContext = owinContext;
        }

        public void SetMimeType(string mimeType)
        {
            _owinContext.Response.ContentType = mimeType;
        }

        public void SetLength(int bytesLength)
        {
            _owinContext.Response.ContentLength = bytesLength;
        }

        public Task WriteBytesAsync(byte[] bytes)
        {
            return _owinContext.Response.WriteAsync(bytes, _owinContext.Request.CallCancelled);
        }
    }
}