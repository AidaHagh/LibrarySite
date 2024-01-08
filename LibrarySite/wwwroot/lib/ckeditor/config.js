/**
 * @license Copyright (c) 2003-2019, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see https://ckeditor.com/legal/ckeditor-oss-license
 */

CKEDITOR.editorConfig = function( config ) {
	config.language = 'fa';
	config.codeSnippet_theme = 'atelier-seaside.light';
	CKEDITOR.config.allowedContent = true;
	config.filebrowserImageUploadUrl = '/file-upload';
	config.extraPlugins = 'codesnippet,defaultform,automationform';
	
	//config.uiColor = '#9b9d9d';
	config.format_tags = 'p;h1;h2;h3;pre';
	config.removeDialogTabs = 'image:advanced;link:advanced';

	config.removePlugins = 'Toolbar';
	config.removeButtons = 'About,Form,Select,Button,Source,Preview,Print,Templates,Save,HiddenField,Find,Expandable,TextField,Textarea,Anchor,Iframe,ImageButton,Radio,Checkbox,Flash,CreateDiv,Language,Smiley,ShowBlocks';

	config.ToolbarGroups = [{ name: 'insert', groups: ['defaultform'] }] ;
};