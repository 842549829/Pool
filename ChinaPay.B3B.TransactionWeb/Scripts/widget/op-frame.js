function SetCwinHeight(){
	var iframeid=document.getElementById("iframe"); //iframe id
	if (document.getElementById){
		if (iframeid && !window.opera){
			if (iframeid.contentDocument && iframeid.contentDocument.body.offsetHeight){
				iframeid.height = iframeid.contentDocument.body.offsetHeight;
			}else if(iframeid.Document && iframeid.Document.body.scrollHeight){
				iframeid.height = iframeid.Document.body.scrollHeight;
			}
		}
	}
}
function drawMenu() {
	$('.menu-client > li').click(function(){
		$(this).toggleClass('active');
	});
	var objMenuLink = $('.menu-manage a, .menu-client a');
}

function drawMenuLinks() {

	$body = (window.opera) ? (document.compatMode == "CSS1Compat" ? $('html') : $('body')) : $('html,body'); //兼容opera画面闪的bug

	var objLoading = $('#loading');
	var objLoadContainer = $('#bd .flow');
	var objMenuLink = $('.menu-manage a, .menu-client a');

	objMenuLink.click(function(event){
		var target = $(event.target);
		if(!target.attr('target')){
			objLoading.show();
			objMenuLink.removeClass('cur');
			$(this).addClass('cur');

			objLoadContainer.load($(this).attr('href'), function(){
				objLoading.hide();
				$body.animate({scrollTop: 0}, 100);
			});
			//console.log(document.compatMode);
			return false;
		}
	});
}
