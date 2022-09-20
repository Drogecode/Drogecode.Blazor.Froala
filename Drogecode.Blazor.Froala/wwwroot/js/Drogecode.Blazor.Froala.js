var dotNetHelper = [];
var editors = [];
function frSetDotNetHelper(froalaId, value) {
    dotNetHelper[froalaId] = value;
}
function frDisposeDotNetHelper(froalaId) {
    delete dotNetHelper[froalaId];
}
function frDisposeEditor(froalaId) {
    editors[froalaId].destroy();
    delete editors[froalaId];
}
function frCreateEditor(elementId, froalaId, contentId, config) {
    // @ts-ignore
    var froalaEditor = new FroalaEditor(elementId, {
        key: config.key,
        toolbarInline: config.toolbarInline,
        charCounterCount: config.charCounterCount,
        // Change save interval (time in milliseconds).
        saveInterval: config.saveInterval,
        // Set the save param.
        saveParam: config.saveParam,
        // Set the save URL.
        saveURL: config.saveUrl,
        // HTTP request type.
        saveMethod: config.saveMethod,
        // Additional save params.
        saveParams: {
            froalaId: froalaId,
            contentId: contentId
        },
        events: {
            'contentChanged': function () {
                dotNetHelper[froalaId].invokeMethodAsync('ContentChanged');
            },
            'click': function () {
                dotNetHelper[froalaId].invokeMethodAsync('Click');
            },
            'blur': function () {
                dotNetHelper[froalaId].invokeMethodAsync('Blur');
            },
            'save.before': function (html) {
                // Before save request is made.
                dotNetHelper[froalaId].invokeMethodAsync('SaveBefore');
            },
            'save.after': function (data) {
                // After successfully save request.
                dotNetHelper[froalaId].invokeMethodAsync('SaveAfter');
            },
            'save.error': function (error, response) {
                // Do something here.
                dotNetHelper[froalaId].invokeMethodAsync('SaveError');
            }
        }
    });
    editors[froalaId] = froalaEditor;
    return froalaEditor;
}
function frSave(froalaId) {
    editors[froalaId].save.save();
}
//# sourceMappingURL=Drogecode.Blazor.Froala.js.map