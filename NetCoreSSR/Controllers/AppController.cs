using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreSSR.Services;

namespace NetCoreSSR.Controllers {
  [Route("/")]
  public class AppController : Controller {
    private VueRenderer _vueRenderer;

    [HttpGet("/")]
    public ContentResult Index () {
      return new ContentResult() {
        Content = this._vueRenderer.HtmlCache,
        ContentType = "text/html",
        StatusCode = 200
      };;
    }

    public AppController (VueRenderer vueRenderer) {
      this._vueRenderer = vueRenderer;
    }
  }
}
