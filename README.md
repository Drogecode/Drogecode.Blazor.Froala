[![Nuget](https://img.shields.io/nuget/v/Drogecode.Blazor.Froala)](https://www.nuget.org/packages/Drogecode.Blazor.Froala/)

[![NuGet Downloads](https://img.shields.io/nuget/dt/Drogecode.Blazor.Froala?label=NuGet%20Downloads)](https://www.nuget.org/packages/Drogecode.Blazor.Froala/)  

# Froala editor

[Froala wysiwyg-editor](https://froala.com/wysiwyg-editor/) is a payed editor for your web projects. This blazor component makes it posible to add it to your blazor wasm projects.

## How to use

You need to add the froala css and js files to your index.html file. This project includes a copy of those files, however, you can also use a copy from a different location.

In the <head> add the .css file

+ `<link href="_content/Drogecode.Blazor.Froala/css/Froala.min.css" rel="stylesheet" />` - Froala css

In the <body> add the .js files

+ `<script src="_content/Drogecode.Blazor.Froala/js/Drogecode.Blazor.Froala.min.js"></script>` - js for this component 
+ `<script src="_content/Drogecode.Blazor.Froala/js/Froala.min.js"></script>` - Froala js

[See the demo index.html for an example](https://github.com/Drogecode/Drogecode.Blazor.Froala/blob/master/Drogecode.Blazor.FroalaDemo/wwwroot/index.html)

## Contributing

This component currently only integrates a small sections of the api options in Froala, if you mis anything, open an issue or PR, with the url to the [event / method / opion](https://froala.com/wysiwyg-editor/docs/api/) you are missing. For obvius reasons PR's will be solved a lot faster than issues.

## FAQ

Q: Has this project any connections with the company behind froala.com?

A: No

Q: Is there a demo how to implement the component?

A: Yes! [here .razor](https://github.com/Drogecode/Drogecode.Blazor.Froala/blob/master/Drogecode.Blazor.FroalaDemo/Pages/Index.razor) and [here .cs](https://github.com/Drogecode/Drogecode.Blazor.Froala/blob/master/Drogecode.Blazor.FroalaDemo/Pages/Index.razor.cs)
 
Q: Why both `FroalaEditorConfig` and `FroalaEditorDetail`?

A: `FroalaEditorConfig` can be used for multiple different instances while `FroalaEditorDetail` is different for every seperate insitance of the Froala wysiwyg-editor

Q: What is inside Froala.min.js?

A: Both `froala_editor.pkgd.min.js"` and `plugins/save.min.js` see [bundleconfig.json](https://github.com/Drogecode/Drogecode.Blazor.Froala/blob/master/Drogecode.Blazor.Froala/bundleconfig.json)

Q: Does the blazor copy of the DOM knows about this component?

A: No, this gives issues and non descriptive errors, but those can be solved.
