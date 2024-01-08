CKEDITOR.plugins.add('defaultform', {

    init: function (editor) {
        editor.ui.addButton('defaultForm', {
            label: "محتوای پیش فرض",
            command: "defaultcommnd"
            //toolbar: "insert"
        })

        //editor.addCommand('defaultcommnd', {
        //    exec: function (editor) {
        //        alert('salam');
        //    }
        //})
    }
});