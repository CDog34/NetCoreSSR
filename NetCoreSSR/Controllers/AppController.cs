using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using JavaScriptEngineSwitcher.ChakraCore;
using JavaScriptEngineSwitcher.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;

namespace NetCoreSSR.Controllers {
  [Route("/")]
  public class AppController : Controller {
    private IJsEngine engine = JsEngineSwitcher.Instance.CreateEngine(ChakraCoreJsEngine.EngineName);

    [HttpGet("/")]
    public ContentResult Index () {
      // Setup JS polyfills.
      engine.Evaluate(
        "var process = { env: { VUE_ENV: 'server', NODE_ENV: 'production' }};" +
        "var console = { log: () => {}, warn: () => {}, error: () => {} };" +
        "this.global = { console: console, process: process };"
      );
      
      // Load Vue library.
      engine.ExecuteFile("ClientResources/libs/vue-2.5.16.js");
      engine.ExecuteFile("ClientResources/libs/vuex-3.0.1.js");
      engine.ExecuteFile("ClientResources/libs/vue-basic-renderer.js");
      
      // Evaluate index.js.
      engine.ExecuteFile("ClientResources/index.server.js");
      
      // Get results.
      var vueVersion = engine.Evaluate("(() => { return Vue.version })()") as string;
      var ssrOutlet = engine.Evaluate("(() => { return htmlString })()") as string;
      var clientScripts = System.IO.File.ReadAllText("ClientResources/index.client.js");
      var html = System.IO.File.ReadAllText("ClientResources/index.html");

      html = html
        .Replace("${ENGINE_NAME}", ChakraCoreJsEngine.EngineName)
        .Replace("${VUE_VERSION}", vueVersion)
        .Replace("${SSR_OUTLET}", ssrOutlet)
        .Replace("${CLIENT_SCRIPT}", clientScripts);
    
      return new ContentResult() {
        Content = html, ContentType = "text/html", StatusCode = 200
      };;
    }
  }
}
