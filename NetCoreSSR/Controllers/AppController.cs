using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;
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

    private class UpdateResponse {
      public int code;
      public string message;
    }
    
    [HttpGet("/update")]
    public JsonResult Update () {
      this._vueRenderer.Render();
      return new JsonResult(new UpdateResponse() {
        code = 0, message = "OK"
      });
    }

    public AppController (VueRenderer vueRenderer) {
      this._vueRenderer = vueRenderer;
    }
  }
}
