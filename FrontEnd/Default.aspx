<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="InfoSupport.KC.OpleidingsplanGenerator.FrontEnd.config" %>
<!doctype html>
<html lang="en">
  <head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, minimum-scale=1, initial-scale=1, user-scalable=yes">

    <title>Opleidingsplan generator</title>
    <meta name="description" content="My App description">

    <link rel="icon" href="/images/favicon.ico">

    <!-- See https://goo.gl/OOhYW5 -->
    <link rel="manifest" href="manifest.json">

    <!-- See https://goo.gl/qRE0vM -->
    <meta name="theme-color" content="#3f51b5">

    <!-- Add to homescreen for Chrome on Android. Fallback for manifest.json -->
    <meta name="mobile-web-app-capable" content="yes">
    <meta name="application-name" content="My App">

    <!-- Add to homescreen for Safari on iOS -->
    <meta name="apple-mobile-web-app-capable" content="yes">
    <meta name="apple-mobile-web-app-status-bar-style" content="black-translucent">
    <meta name="apple-mobile-web-app-title" content="My App">

    <!-- Homescreen icons -->
    <link rel="apple-touch-icon" href="/images/manifest/icon-48x48.png">
    <link rel="apple-touch-icon" sizes="72x72" href="/images/manifest/icon-72x72.png">
    <link rel="apple-touch-icon" sizes="96x96" href="/images/manifest/icon-96x96.png">
    <link rel="apple-touch-icon" sizes="144x144" href="/images/manifest/icon-144x144.png">
    <link rel="apple-touch-icon" sizes="192x192" href="/images/manifest/icon-192x192.png">

    <!-- Tile icon for Windows 8 (144x144 + tile color) -->
    <meta name="msapplication-TileImage" content="/images/manifest/icon-144x144.png">
    <meta name="msapplication-TileColor" content="#3f51b5">
    <meta name="msapplication-tap-highlight" content="no">

      <script>

    (function () {
        BackendAdress = "<%= System.Web.Configuration.WebConfigurationManager.AppSettings["BackendAdress"] %>";
    })();

      // Setup Polymer options
      window.Polymer = {
        dom: 'shadow',
        lazyRegister: true
      };

      // Load webcomponentsjs polyfill if browser does not support native Web Components
      (function() {
        'use strict';

        var onload = function() {
          // For native Imports, manually fire WebComponentsReady so user code
          // can use the same code path for native and polyfill'd imports.
          if (!window.HTMLImports) {
            document.dispatchEvent(
              new CustomEvent('WebComponentsReady', {bubbles: true})
            );
          }
        };

        var webComponentsSupported = (
          'registerElement' in document
          && 'import' in document.createElement('link')
          && 'content' in document.createElement('template')
        );

        if (!webComponentsSupported) {
          var script = document.createElement('script');
          script.async = true;
          script.src = 'bower_components/webcomponentsjs/webcomponents-lite.min.js';
          script.onload = onload;
          document.head.appendChild(script);
        } else {
          onload();
        }
      })();

      // Load pre-caching Service Worker
      //if ('serviceWorker' in navigator) {
      //  window.addEventListener('load', function() {
      //    navigator.serviceWorker.register('/service-worker.js');
      //  });
      //}
    </script>
    

    <link rel="import" href="src/opleidingsplan-app.html">

    <style>
      body {
        margin: 0;
        font-family: 'Roboto', 'Noto', sans-serif;
        line-height: 1.5;
        min-height: 100vh;
        background-color: #eeeeee;
      }

      #IEHeader {
          position: fixed;
          top: 0;
          left: 0;
          width: 100%;
          background-color: #FF6666;
          z-index: 99999;
          padding: 20px;
          text-align: center;
          font-size: 22px;
          display: none;
      }
    </style>
  </head>
  <body>
    <div id="IEHeader">Deze website is mogelijk niet goed te gebruiken in Internet Explorer. Gebruik een andere browser zoals: Chrome, Firefox of Edge</div>
    <opleidingsplan-app></opleidingsplan-app>

  </body>

    <script>
        function GetIEVersion() {
            var sAgent = window.navigator.userAgent;
            var Idx = sAgent.indexOf("MSIE");

            // If IE, return version number.
            if (Idx > 0)
                return parseInt(sAgent.substring(Idx + 5, sAgent.indexOf(".", Idx)));

                // If IE 11 then look for Updated user agent string.
            else if (!!navigator.userAgent.match(/Trident\/7\./))
                return 11;

            else
                return 0; //It is not IE
        }

        if (GetIEVersion() > 0) {
            document.querySelector("#IEHeader").style.display = "block";
        }
    </script>