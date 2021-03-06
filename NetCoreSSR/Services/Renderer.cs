﻿using JavaScriptEngineSwitcher.ChakraCore;
using JavaScriptEngineSwitcher.Core;
using System.IO;

namespace NetCoreSSR.Services {
  public class VueRenderer {
    public string HtmlCache = null;
    public IJsEngine Engine = null;

    public void Render () {
      var engine = this.Engine;
      
      // Evaluate index.js.
      engine.ExecuteFile("ClientResources/index.server.js");
      
      // Get results.
      var vueVersion = engine.Evaluate("(() => { return Vue.version })()") as string;
      var ssrOutlet = engine.Evaluate("(() => { return renderedHTML })()") as string;
      var clientScripts = File.ReadAllText("ClientResources/index.client.js");
      var html = File.ReadAllText("ClientResources/index.html");

      html = html
        .Replace("${ENGINE_NAME}", ChakraCoreJsEngine.EngineName)
        .Replace("${VUE_VERSION}", vueVersion)
        .Replace("${SSR_OUTLET}", ssrOutlet)
        .Replace("${CLIENT_SCRIPT}", clientScripts);

      this.HtmlCache = html;
    }

    private void initEngine () {
      var engine = JsEngineSwitcher.Instance.CreateEngine(ChakraCoreJsEngine.EngineName);
      
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

      this.Engine = engine;
    }

    public VueRenderer () {
      this.initEngine();
      this.Render();
    }
  }
}
