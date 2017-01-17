# Snippets Markdown Monster Add-in

<img src="SnippetsAddin.png" Height="128" />

This project provides a simple snippet manager addin for the Markdown [Monster Markdown Editor and Weblog Publisher](https://markdownmonster.west-wind.com). The addin lets you embed saved text snippets with optional C# expressions into your Markdown Monster document. 

You can store and select from a list of text based snippets that you can embed into your Markdown or HTML documents by double clicking, pressing `ENTER` or `SPACE`.

![](ScreenShot.png)

Snippets make a great use case for:

* Signatures
* Page or Support Templates
* Prefilled Bug Reports
* Timestamping documents

### Embed C# Code Expressions
Snippets can contain embedded C# code expressions using `{{ expression }}` syntax which is evaluated when the snippet is rendered.

For example the following:

```html
<div class="small">
   created by, Rick Strahl, on {{DateTime.Now.ToString("MMM dd, yyyy")}}
</div>   
```

embeds a date into the snippet when it's created. Snippets can embed **any** text since Markdown supports both plain text as well as HTML markup as in the example above.

You also get access to the full Addin model that exposes a large chunk of Markdown Monsters active document, editor and UI using a `Model property. 

For example, if you want to get the current filename:

```Markdown
Main Window Title:  {{Model.Window.Title}}. 

Time is: {{DateTime.Now}}

Filename: {{Model.ActiveDocument.Filename}}
```

You only get to apply expressions, but that gives you a fair bit of functionality you can work with.

> #### Early pre-release
> This version is a pre-release version so installation and configuration is manual for now. Please see instructions below.

### 

### How it works

## Configuration

### Related Links

* [Markdown Monster](https://markdownmonster.west-wind.com)
* [Creating a Markdown Monster Addin](https://markdownmonster.west-wind.com/docs/_4ne0s0qoi.htm)
