// JavaScript Document

(function($) {
	//封装替换select下拉
	$.fn.custSelect = function(options, callback){

		var defaults = {
				width: 'auto',
				callback: function(){}  //callback 直接返回选中的 value 值
			};

		var opts = $.extend(defaults, options);

		return this.each(function(){
			//$(this).click(function(){alert($(this).html())});
			var obj = $(this);
			var options = $('option', obj);  // 获取所有 option
			var selected = obj.children('option[selected="selected"]');  // 获取当前选中 option
			var id = obj.attr('id');

			if (!$(obj).hasClass('custed')){
				// 生成模拟容器
				obj.after('<dl id="dl_'+id+'" class="dropdown"></dl>');
				var dl = obj.next('dl');
				dl.append('<dt><button class="btn class4"><i class="icon icon-down-open fr"></i>' + selected.text() + '</button><span class="value">' + selected.val() + '</span></dt>');
				dl.append('<dd><ul></ul></dd>');

				//模拟下拉展开和收缩
				dl.children('dt').click(function(){
					var el = $(this).next('dd');
					if(!el.is(':visible')){
						$('dl.dropdown dd').hide();
					}
					el.slideToggle(50);
					//$(this).next('dd').show(50);
					return false;
				});

				$(document).bind('click', function(e) {
					var $clicked = $(e.target);
					if (! $clicked.parents().hasClass("dropdown"))
					 $("dl.dropdown dd").hide();
				});

				obj.addClass('custed');
				obj.hide();
			} else {
				$(obj).next('dl').find('ul').empty();
			}

			var dl = obj.next('dl');

			//生成模拟下拉选项
			options.each(function(){
				dl.find('ul').append('<li>' + $(this).text() + '<span class="value">'+ $(this).val() +'</span></li>')
			});

			//修正宽度及显示
			if (opts.width == 'auto'){

				var dlWidth = dl.find('button').width();

				var ddWidth = dl.children('dd').width();

				if(dlWidth < ddWidth) {
					dl.find('button').css('width', ddWidth);
				}else{
					dl.find('button').css('width', dlWidth);
					dl.find('ul').css('width', dlWidth);
				}
			} else {
				dl.find('button').css('width', opts.width);
				dl.find('ul').css('width', opts.width);
			}

			if(dl.find('li').length > 10){
				dl.find('ul').css({'height':'352px', 'overflow-y':'scroll'}).find('li').css('padding-right','18px');
			}
			dl.children('dd').hide();



			//实现下拉选择
			dl.find('li').click(function(){
				var text = $(this).html();
				var value = $(this).find('span.value').html();
				dl.find('button').html('<i class="icon icon-down-open fr"></i>' + text);
				dl.children('dd').hide();
				obj.val(value);
				opts.callback.call(obj);
			});

		});
	}
	$('select').custSelect({width:'158px'});

	//封装替换checkbox&radio
	$.fn.custCheckBox = function(options){

		var defaults = {
				disable_all:	false,				//disables all the elements
				hover:	true,						//adds a hover state to the tag
				wrapperclass:	"check",			//the class name of the wrapper tag
				callback:	function(){}			//a click event call back
			};
		//override defaults
		var opts = $.extend(defaults, options);

		return this.each(function() {
			var obj = $(this);

			if (!$(obj).hasClass('custed')) {

				$.fn.buildbox = function(thisElm){

					$(thisElm).css({display:"none"}).after("<span class=\"cust_checkbox\">&nbsp;&nbsp;&nbsp;&nbsp;</span>");

					var isChecked = $(thisElm).attr("checked");
					var boxtype = $(thisElm).attr("type");
					var disabled = $(thisElm).attr("disabled");

					if(boxtype === "checkbox")
					{
						$(thisElm).next("span").addClass("checkbox");
						if(disabled || opts.disable_all){boxtype = "checkbox_disabled";}
					}
					else
					{
						$(thisElm).next("span").addClass("radio");
						if(disabled || opts.disable_all){boxtype = "radio_disabled";}
					}

					if(isChecked)
						$(thisElm).next("span").addClass("cust_"+boxtype+"_on");
					else
						$(thisElm).next("span").addClass("cust_"+boxtype+"_off");

					if(opts.disable_all)
						$(thisElm).attr("disabled","disabled");
				};

				//build the boxes
				$.fn.buildbox($(obj));
				$(obj).addClass('custed');
			}


			//模拟label点击
			$(obj).next("span").next("label").unbind().click(function(){

				if(!opts.disable_all)
				{
					var custbox = $(this).prev("span");
					var boxtype = $(custbox).prev("input").attr("type");
					var boxname = $(custbox).prev('input').attr('name');
					var disabled = $(custbox).prev("input").attr("disabled");

					if($(custbox).hasClass("checkbox"))
					{
						if($(custbox).hasClass("cust_"+boxtype+"_off") && !disabled)
						{
							$(custbox).removeClass("cust_"+boxtype+"_off").addClass("cust_"+boxtype+"_on").prev("input").attr("checked","checked"); //turn on
						}

						else if(!disabled)
						{
							$(custbox).removeClass("cust_"+boxtype+"_on").addClass("cust_"+boxtype+"_off").prev("input").removeAttr("checked"); //turn off
							$(custbox).removeClass("cust_"+boxtype+"_hvr");
						}


					}
					else if(!disabled)
					{
						$('input[type="radio"][name="'+boxname+'"]').removeAttr("checked").next('.cust_checkbox').removeClass("cust_"+boxtype+"_on").addClass("cust_"+boxtype+"_off");
						$(custbox).removeClass("cust_"+boxtype+"_off").addClass("cust_"+boxtype+"_on").prev("input").attr("checked","checked"); //turn on
						$(custbox).removeClass("cust_"+boxtype+"_hvr");
					}

					opts.callback.call($(obj));

				}

			}).hover(function(){
				var custbox = $(this).prev("span");
				if($(custbox).hasClass("cust_checkbox_on") && opts.hover)
					$(custbox).addClass("cust_checkbox_hvr");
				else if($(custbox).hasClass("cust_radio_on") && opts.hover)
					$(custbox).addClass("cust_radio_hvr");

			},function(){
				var custbox = $(this).prev("span");
				if($(custbox).hasClass("cust_checkbox_on") && opts.hover)
					$(custbox).removeClass("cust_checkbox_hvr");
				else if($(custbox).hasClass("cust_radio_on") && opts.hover)
					$(custbox).removeClass("cust_radio_hvr");

			});

			//模拟checkbox和radio的点击
			$(obj).next("span").unbind().click(function(){

				if(!opts.disable_all)
				{
					var boxtype = $(this).prev("input").attr("type");
					var boxname = $(this).prev('input').attr('name');
					var disabled = $(this).prev("input").attr("disabled");

					if($(this).hasClass("checkbox"))
					{
						if($(this).hasClass("cust_"+boxtype+"_off") && !disabled)
							$(this).removeClass("cust_"+boxtype+"_off").addClass("cust_"+boxtype+"_on").prev("input").attr("checked","checked"); //turn on
						else if(!disabled)
						{
							$(this).removeClass("cust_"+boxtype+"_on").addClass("cust_"+boxtype+"_off").prev("input").removeAttr("checked"); //turn off
							$(this).removeClass("cust_"+boxtype+"_hvr");
						}
					}
					else if(!disabled)
					{
						$('input[type="radio"][name="'+boxname+'"]').removeAttr("checked").next('.cust_checkbox').removeClass("cust_"+boxtype+"_on").addClass("cust_"+boxtype+"_off");
						$(this).removeClass("cust_"+boxtype+"_off").addClass("cust_"+boxtype+"_on").prev("input").attr("checked","checked"); //turn on
					}

					opts.callback.call($(obj));

				}
			}).hover(function(){
				if($(this).hasClass("cust_checkbox_on") && opts.hover)
					$(this).addClass("cust_checkbox_hvr");
				else if($(this).hasClass("cust_radio_on") && opts.hover)
					$(this).addClass("cust_radio_hvr");
			},function(){
				if($(this).hasClass("cust_checkbox_on") && opts.hover)
					$(this).removeClass("cust_checkbox_hvr");
				else if($(this).hasClass("cust_radio_on") && opts.hover)
					$(this).removeClass("cust_radio_hvr");
			});

		});
	};

})(jQuery);

function areaSel(areaData, args) {
    var position = $(".areaSelect").position();
    $('.areaSelect').after('<div class="firstAreaClass"></div>');
    $(".firstAreaClass").css({ "position": "absolute", "top": position.top + $(".areaSelect").height(), "left": position.left });
    var args = args || '';
    el = args.el || $('.areaSelect'),
		selBox = args.box || el.next('.firstAreaClass'),
    //selBox = args.box || el.after('<div class="firstAreaClass" style="display:none;"></div>'),
		data = args.data || areaData,
		tmp = '',
		dlTmp = '';
    el.append('<input type="hidden" name="" class="areaData" />')
    var textInput = el.find('.areaData');
    for (var i = 0, l = data.length; i < l; i++) {
        dlTmp = '<dl><dt code="' + data[i].Id + '">' + data[i].name + '</dt>';
        var ddTmp = '';
        if (data[i].province.length > 0) {
            for (var j = 0, len = data[i].province.length; j < len; j++) {
                ddTmp += '<dd><a href="javascript:void(0);" code="' + data[i].province[j].Id + '">' + data[i].province[j].name + '</a></dd>';
            }
            tmp = tmp + dlTmp + ddTmp + '</dl>';
        }
    }
    var cityBox = '<div class="cityBox"></div>',
		areaBox = '<div class="areaBox"></div>';
    selBox.html(tmp).append(cityBox).append(areaBox);
    var bigAreaName,
		provinceName,
		cityName,
		areaName;
    function createCity(ele) {
        var _index = ele.parent().index(),
			_indexP = ele.index() - 1,
			cityTmp = '',
			provinceEl = el.find('.provinceName');
        if (ele.text() != provinceName) {
            provinceName = ele.text();
            provinceEl.html(provinceName).attr('code', ele.find('a').attr('code'));
            if (!el.find('.bigAreaName')[0]) {
                provinceEl.before('<span class="bigAreaName" code=' + ele.parent().find('dt').attr('code') + '>' + ele.parent().find('dt').text() + '</span>')
            } else {
                el.find('.bigAreaName').html(ele.parent().find('dt').text()).attr('code', ele.parent().find('dt').attr('code'));
            }
            el.find('.cityName').remove();
            el.find('.areaName').remove();
        } else {
            provinceEl.html(ele.text()).attr('code', ele.find('a').attr('code'));
        }
        if (data[_index].province[_indexP].city.length > 0) {
            for (var i = 0, l = data[_index].province[_indexP].city.length; i < l; i++) {
                cityTmp += '<a href="javascript:void(0);" code="' + data[_index].province[_indexP].city[i].Id + '">' + data[_index].province[_indexP].city[i].name + '</a>';
            }
        }
        ele.parent('dl').parent('div').find('a').removeClass('curSel');
        ele.children('a').addClass('curSel');
        selBox.find('.cityBox').html(cityTmp).show().addClass('curDiv').find('a').click(function () {
            createArea(_index, _indexP, $(this));
            $(this).parent('.cityBox').find('a').removeClass('curSel');
            $(this).addClass('curSel');
            $(this).parent('.cityBox').removeClass('curDiv');
        });
    };
    function createArea(_index, _indexP, ele) {
        var _indexS = ele.index(),
			areaTmp = '',
			txtBox = el.find('.cityName');
        if (ele.text() != cityName) {
            cityName = ele.text();
            txtBox.html(cityName);
            el.find('.areaName').remove();
        } else {
            txtBox.html(ele.text());
        }
        if (!txtBox.length) {
            el.find('.provinceName').after('<span class="cityName" code=' + ele.attr('code') + '>' + ele.text() + '</span>');
        } else {
            txtBox.html(ele.text()).attr('code', ele.attr('code'));
        }
        if (data[_index].province[_indexP].city[_indexS].area.length > 0) {
            for (var i = 0, l = data[_index].province[_indexP].city[_indexS].area.length; i < l; i++) {
                areaTmp += '<a href="javascript:void(0);" code="' + data[_index].province[_indexP].city[_indexS].area[i].Id + '">' + data[_index].province[_indexP].city[_indexS].area[i].name + '</a>';
            }
        }
        selBox.find('.areaBox').html(areaTmp).show().addClass('curDiv').find('a').click(function () {
            var txtBox = el.find('.areaName'),
				_t = $(this);

            $(this).parent('.areaBox').removeClass('curDiv');
            $(this).parent('.areaBox').find('a').removeClass('curSel');
            $(this).addClass('curSel');
            selBox.hide();

            if (!txtBox.length) {
                el.find('.cityName').after('<span class="areaName" code=' + _t.attr('code') + '>' + _t.text() + '</span>');
            } else {
                txtBox.html(_t.text()).attr('code', _t.attr('code'));
            }
        });
    };
    el.click(function () {
        selBox.slideToggle(50);
    });
    selBox.on('click', 'dd', function () {
        createCity($(this));
        selBox.find('.areaBox').hide();
    }).on('click', function () {
        var data = { "AreaCode": el.find('.bigAreaName').attr('code'), "AreaName": el.find('.bigAreaName').text(), "ProvinceCode": el.find('.provinceName').attr('code'), "ProvinceName": el.find('.provinceName').text(), "CityCode": el.find('.cityName').attr('code'), "CityName": el.find('.cityName').text(), "CountyCode": el.find('.areaName').attr('code'), "CountyName": el.find('.areaName').text() };
        textInput.val(JSON.stringify(data));
    });
}