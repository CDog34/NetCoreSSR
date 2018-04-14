using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using JavaScriptEngineSwitcher.ChakraCore;
using JavaScriptEngineSwitcher.Extensions.MsDependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace NetCoreSSR {
  class HtmlOutputFormatter : StringOutputFormatter {
    public HtmlOutputFormatter () {
      SupportedMediaTypes.Add("text/html");
    }
  }
  
  public class Startup {
    public IConfiguration Configuration { get; }

    public Startup (IConfiguration configuration) {
      Configuration = configuration;
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices (IServiceCollection services) {
      // Add ChakraCore.
      services.AddJsEngineSwitcher(options => options.DefaultEngineName = ChakraCoreJsEngine.EngineName)
        .AddChakraCore();
      
      // HTML Response Formatter for [Produces("text/html")].
      services.Configure<MvcOptions>(options =>
        options.OutputFormatters.Add(new HtmlOutputFormatter())
      );

      services.AddMvc();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure (IApplicationBuilder app, IHostingEnvironment env) {
      if (env.IsDevelopment()) {
        app.UseDeveloperExceptionPage();
      }
      app.UseMvc();
    }
  }
}
