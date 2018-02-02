$(function() {
	/*
	 * 以下均为测试调用
	 * */
	
	
	$( ".s-dbl_select" ).dblSelect();
	
	$( "#s-date" ).priceDate({
		onPrint: function( dateJson ) {
			
			var jsonData = { 
				"departure": "CTU",
				"arrival": "PEK",
				"startDate": dateJson.start,
				"endDate": dateJson.end
			};
			
			$.post(
				"", 
				jsonData,
				function( arr ) {
					// arr 为模拟数据
					var arr = [
						{"Date":"2012-07-12","Fare":"310"},
						{"Date":"2012-07-13","Fare":"340"},
						{"Date":"2012-07-14","Fare":"300"},
						{"Date":"2012-07-15","Fare":"340"},
						{"Date":"2012-07-16","Fare":"3100"},
						{"Date":"2012-07-17","Fare":"1340"},
						{"Date":"2012-07-18","Fare":"3410"},
						{"Date":"2012-07-19","Fare":"1300"},
						{"Date":"2012-07-20","Fare":"240"},
						{"Date":"2012-07-21","Fare":"10"},
						{"Date":"2012-07-22","Fare":"320"},
						{"Date":"2012-07-23","Fare":"110"}
					];
					
					for( var i = 0; i < arr.length; i ++ ) {
						var data = arr[i], id = "#s-price_date-"+ data.Date, price = data.Fare;
						
						$( id ).find( ".price" ).html( "￥" + price );
					}
				}
			);
			
		}
	});
});






(function( $ ) {
/*
 * 双向选测列表
 * 
 */
$.fn.dblSelect = function( options ) {
	return this.each(function() {
		new dblSelect( this, options );
	});
};
function dblSelect( target ) {
	this.target = target;
	
	this.opt = {
		disabled: "s-dbl_select_disabled"
	};
	
	$.extend( this.opt, target );
	
	this.Init();
}
dblSelect.prototype = {
	Init: function() {
		var elems = $( this.target ).find( "div" );
		
		// 左侧待选列表
		this.hold_list 	= elems.eq( 0 );
		// 右侧已选列表
		this.join_list 	= elems.eq( 1 );
		
		this.all_choose = $( this.target ).find( ".s-all_choose" );
		this.all_remove = $( this.target ).find( ".s-all_remove" );
		
		this.DisabledList();
		
		this.Bind();
	},
	
	// 绑定交互事件
	Bind: function() {
		var _this = this;
		
		// 将左侧项添加到右侧
		this.hold_list.find( "span" ).click(function() {
			var left_cloner = _this.Add( this );
			
			if( left_cloner && left_cloner.length ) {
				_this.join_list.append( left_cloner );
				
				// 同时绑定该新加项目的交互事件
				left_cloner.bind( "click", function() {
					_this.Remove( this );
				})
			}
		});
		
		// 将右侧项添加到左侧
		// 绑定默认情况下的右侧列表交互事件
		this.join_list.find( "span" ).click(function() {
			_this.Remove( this );
		});
		
		this.all_choose.click(function() {
			
			_this.hold_list.find( "span" ).each(function() {
				$( this ).triggerHandler( "click" );
			});
		});
		
		// 删除所有右侧已选项
		// 直接触发右侧项的点击事件
		this.all_remove.click(function() {
			
			_this.join_list.find( "span" ).each(function() {
				$( this ).triggerHandler( "click" );
			});
		});
	},
	
	// 判断左侧项是否已经选择
	DisabledList: function() {
		// 判断右侧列表
		// 如果右侧存在，则将右侧对应项的属性设置为 disabled
		var _this = this;
		
		this.join_list.find( "span" ).each(function() {
			// 存在元素则返回待选列表中的该元素
			// 否则返回null
			_this.Disabled( _this.FindHoldList( this ) );
		});
	},
	
	// 查找是否在左侧列表中
	FindHoldList: function( target ) {
		var list = this.hold_list.find( "span" );
		
		for( var i = 0; i < list.length; i ++ ) {
			var item = list[ i ];
			
			if( $( target ).attr( "value" ) === $( item ).attr( "value" ) ) {
				return item;
			}
		}
		
		return null;
	},
	
	// 将右侧点击项设置为不可点击
	Disabled: function( target ) {
		if( !target || !$( target ).length ) return;
		
		$( target ).attr( "disabled", "disabled" ).addClass( this.opt.disabled );
	},
	
	// 将右侧点击项设置为可点击
	UnDisabled: function( target ) {
		if( !target || !$( target ).length ) return;
		
		$( target ).removeAttr( "disabled" ).removeClass( this.opt.disabled );
	},
	
	// 判断是否将点击项加入到已选列表容器中
	Add: function( target ) {
		var list = this.join_list.find( "span" ), cloner = null;
		
		for( var i = 0; i < list.length; i ++ ) {
			var item = list[ i ];
			
			if( $( target ).attr( "value" ) === $( item ).attr( "value" ) ) {
				return null;
			}
		}
		
		cloner = $( target ).clone();
				
		this.Disabled( target );
		
		return cloner;
	},
	
	Remove: function( target ) {
		var left_target = this.FindHoldList( target );
		
		if( left_target && $( left_target ).length ) {
			this.UnDisabled( left_target );
			
			$( target ).remove();
		}
	}
};
	
})( jQuery );


function date( str ) {
	if( str === undefined ) {
		this.date = new Date();
	} else {
		this.date = new Date( str );
	}
	
	return this;
}
date.prototype = {
	year: function() {
		return this.date.getFullYear();
	},
	month: function() {
		var m = this.date.getMonth() + 1 + "";
		m = m.length < 2 ? "0" + m : m;
		
		return m;
	},
	day: function() {
		var d = this.date.getDate() + "";
		d = d.length < 2 ? "0" + d : d;
		
		return d;
	},
	week: function() {
		return this.date.getDay() === 0 ? 7 : this.date.getDay();
	},
	
	// 把星期数字转换为中文
	week2CH: function( info_week ) {
		switch( info_week ) {
			case 1: 
				return "周一"; 
				break;
			case 2: 
				return "周二"; 
				break;
			case 3: 
				return "周三"; 
				break;
			case 4: 
				return "周四"; 
				break;
			case 5: 
				return "周五"; 
				break;
			case 6: 
				return "周六"; 
				break;
			default: 
				return "周日"; 
		}
	}
};


(function( $, date ) {
/*
 * 价格日历模块
 * 
 */
$.fn.priceDate = function( options ) {
	return this.each(function() {
		new priceDate( this, options );
	});
}
function priceDate( target, options ) {
	this.target = target;
	
	this.opt = {
		current	: "s-price_date_current",
		// 显示位置数量
		total	: 8,
		// 当前日期在显示区域的位置
		curIndex: 3,
		// 点击左右移动按钮，重新刷新的个数
		step	: 2,
		
		onPrint	: function() {}
	};
	
	$.extend( this.opt, options );
	
	this.Init();
}
priceDate.prototype = {
	Init: function() {
		this._index = 0;
		
		this.cont = $( this.target ).find( "tr" ).empty();
		
		this._prev = $( this.target ).find( ".s-prev-loop" );
		this._next = $( this.target ).find( ".s-next-loop" );
		
		// 输出初始化日历信息
		this.Print();
		
		this.Bind();
	},
	
	/*
	<td>
		<a href="javascript:void(0);">
			<span class="date">12-09</span>
			<span class="week">周五</span>
			<span class="price">￥579</span>
		</a>
	</td>
	 * */
	// 绘制基础 html 
	Print: function() {
		
		// 距离目标天数的 date
		function _fromDate( num ) {
			var n = num === undefined ? 0 : num, 
				// 当前的时间戳
				now = ( new Date() ).getTime(),
				// 距离今天说
				time = now + 1000 * 3600 * 24 * n;
			
			return new Date( time );
		}
		
		var dateArr = [], arr = [], start = this._index, end = start + this.opt.total;
		
		for( var i = start; i < end; i ++ ) {
			var _fd = _fromDate( i - this.opt.curIndex ), dAPI = new date( _fd );
			
			var info_date = dAPI.month() + "-" + dAPI.day(), info_week = dAPI.week(), info_week_ch = dAPI.week2CH( info_week );
			
			var id = "s-price_date-" + dAPI.year() + "-" + info_date;
			
			dateArr.push( info_date );
			
			var curStyle = ( new Date ).getTime() === _fd.getTime() ? "s-date_now" : "";
			
			arr.push(
				'\
					<td id="'+ id +'" class="'+ curStyle +'">\
						<a href="javascript:void(0);">\
							<span class="date">'+ info_date +'</span>\
							<span class="week">'+ info_week_ch +'</span>\
							<span class="price">-</span>\
						</a>\
					</td>\
				' );
		}
			
		this.cont.html( arr.join( "" ) );
		
		var dateJson = {
			start: dateArr[0], end: dateArr[dateArr.length-1]
		};
		
		this.opt.onPrint.call( this.target, dateJson );
	},
	
	Bind: function() {
		var _this = this;
		
		this._prev.click(function() {
			_this._index = _this._index - _this.opt.step;
			
			_this.Print();
			return false;
		});
		
		this._next.click(function() {
			_this._index = _this._index + _this.opt.step;
			
			_this.Print();
			return false;
		});
	}
}

})( jQuery, date );
