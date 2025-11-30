using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IZoomAuthService
    {
        /// <summary>
        /// Get cached Zoom OAuth access token or request a new one if expired.
        /// </summary>
        Task<string> GetAccessTokenAsync(CancellationToken ct);
    }

}
