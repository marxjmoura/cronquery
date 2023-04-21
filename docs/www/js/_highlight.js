import $ from 'jquery'
import hljs from 'highlight.js/lib/core'
import csharp from 'highlight.js/lib/languages/csharp'
import js from 'highlight.js/lib/languages/javascript'
import json from 'highlight.js/lib/languages/json'

hljs.registerLanguage('csharp', csharp)
hljs.registerLanguage('js', js)
hljs.registerLanguage('json', json)

$('pre code').each(function (i, code) {
  hljs.highlightBlock(code);
});
