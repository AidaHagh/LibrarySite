CKEDITOR.plugins.add('automationform', {

    init: function (editor) {
        editor.ui.addButton('automationform', {
            label: "فرم های اداری",
            command: "automationform"
            //toolbar: "insert"
        })


    }
});