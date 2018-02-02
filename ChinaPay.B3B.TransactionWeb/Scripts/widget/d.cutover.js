/**
 * Cutover
 * Deo: denglingbo@126.com
 * Date: 2011.10.12
 * 
 * DOM type 1: 
 * 	<div> -- target: required
 *		<ul>
 *			[ <li></li> ... ] -- items
 *		</ul>
 * 	</div>
 *
 */
(function( $ ) {
	
var elemWidth = 0, elemHeight = 0;
	
$.fn.cutover = function( opt ) {
	this.each(function() {
		var c = new $.cutover( this, opt );
		
		c.init();
	});
};

$.cutover = function( elem, opt ) {
	this.elem = elem;
	
	// Private
	this._len 		= 0; 	// Switch items length
	this._existBar	= null;
	this._btns 		= null; // Switch buttons
	this._items 	= null; // Switch images list
	this._prev		= 0; // Clicked item
	
	this.opt = {
		// click, mouseover
		event 		: 'click',
		
		// Use first if width not null
		width		: null,
		
		// Use first if height not null 
		height		: null,
		
		index		: 0,
		
		// Auto play
		auto  		: false,
		
		// Switch wait when auto play 
		wait  		: 5000,
		
		// Show and hide speed
		speed 		: 400,
		
		// Show, FadeIn, Slide
		effect		: 'fadeIn',
		
		// If slide
		// left or top
		direction	: 'left',
		
		barOut		: false,
		
		// If event: mouseover
		// page jump when click this btn
		clickJump	: false
	};
	
	$.extend( this.opt, opt );
};

$.cutover.prototype = {
	// Auto timer
	timer : null,
	
	classes : {
		wrap 	: 'expack_cutover',
		bar	 	: 'expack_cutover_bar',
		btn	 	: 'expack_cutover_button',
		curBtn	: 'expack_cutover_current_button'
	},
	
	init: function() {
		var _this = this, elem = $( this.elem );
		
		// Create a container
		elem.wrapInner( '<div class="'+ this.classes.wrap +'"></div>' );
		
		var wrap = $( '.' + this.classes.wrap, elem );
		
		// Get current content items
		this._items = this.getNodes();
		
		this._len = this._items.length;

		elemWidth = this.opt.width == null ? elem.width() : this.opt.width, 
		
		elemHeight = this.opt.height == null ? elem.height() : this.opt.height;
		
		wrap.css({ 'width': elemWidth, 'height': elemHeight, 'overflow': 'hidden' });
		
		// Create a move container when effect is slide
		if( this.opt.effect === 'slide' ) {
			wrap.wrapInner( '<div class="expack_cutover_slide" style="position: absolute;"></div>' );
			
			this._slide = $( '.expack_cutover_slide', elem );
		}
		
		this.setConfig();
		
		// Get and set the control buttons
		this.setBar();
		
		this._len = this._btns.length;
		
		// Only one item
		if( this._len <= 1 ) {
			this._btns.hide();
			return false;
		}
		
		this._btns.each(function( i ) {
			if( !_this._items.eq( i ).length ) {
				var temp = null;
				
				_this._items.parent().append( temp = $( '<div></div>' ) );
				
				$.merge( _this._items, temp );
			}
		});
		
		this._items.css({ 'width': elemWidth, 'height': elemHeight, 'overflow': 'hidden' });
		
		// To bind events
		this.bind();

		return this;
	},
	
	// Get the control menu and the tab content
	getNodes: function() {
		var elem = $( '.' + this.classes.wrap, this.elem ),
			
			first = elem.children().not( 'script' ).first();
			
			last = first.next();
		
		if( last.length ) {
			// Get the first children
			var contentFirst = last.children().not( 'script' ).first();
			
			this._existBar = first;
			
			return $.merge( contentFirst, contentFirst.not( 'script' ).siblings() );
		} else {
			return first.find( 'li' );
		}
	},
	
	// Set the cutover items
	// z-index and position
	setConfig: function() {
		var _this = this, typeFunc = {}, len = this._len;
		
		// Show or fadeIn settings
		typeFunc.show = typeFunc.fadeIn = function( target, i ) {
			if( i !== 0 ) {
				$( target ).hide();
			}
		};
		
		// Slide settings
		typeFunc.slide = function( target, i ) {
			var widthMax = $( _this.elem ).outerWidth(), heightMax = $( _this.elem ).outerHeight();
			
			var pos = i * ( _this.opt.direction === 'left' ? widthMax : heightMax );
			
			$( target ).css({ 'position' : 'absolute', 'z-index' : len - i });
			
			$( target ).css( _this.opt.direction, pos ).data( 'pos', pos );
		};

		this._items.each(function( i ) {
			typeFunc[ _this.opt.effect ]( this, i );
		});
	},
	
	// Set cutover switch bar
	setBar: function() {
		var _this = this, btnDelay = null;
		
		// Js add button menu
		if( !this._existBar ) {
			var bar = ['<ol class="'+ this.classes.bar +'">' ];
			
			for( var i = 0; i < this._len; i ++ ) {
				bar.push( '<li><a class="'+ this.classes.btn +'">'+ ( i + 1 ) +'</a></li>' );
			}
			
			bar.push( '</ol>' );	
			
			// Add the bar into cutover element by JS create
			if( this.opt.barOut ) {
				$( this.elem ).append( bar.join('') );
			} else {
				$( '.'+ this.classes.wrap, this.elem ).append( bar.join('') );
			}

			this._btns = $( '.'+ this.classes.btn, this.elem );
		} 
		// Use customer button menu
		else {
			bar = this._existBar;
			
			bar.addClass( this.classes.bar );
			
			$( this.elem ).prepend( bar );
			
			this._btns = $( 'a', bar ).addClass( this.classes.btn );
		}
		
		if( _this.opt.clickJump && _this.opt.event == 'mouseover' ) {
			bar.find( 'a' ).each(function( i ) {
				var li = _this._items.eq( i );
				
				if( li && li.find( 'a' ).eq( 0 ) ) {
					var btnlink = li.find( 'a' ).eq( 0 );
					
					var item_href = btnlink.attr( 'href' ),
						item_target = btnlink.attr( 'target' );
					
					$( this ).attr({ 'href': item_href, 'target': item_target });
				}
			});
		}
		
		// Set the bar
		var bar = $( '.'+ this.classes.bar, this.elem ).css({ 'z-index': this._btns.length + 1 });
		
		// Add default start
		this._btns.each(function( i ) {
			_this.bindContent( i );
			
			this.index = i;
			
			if( i == _this.opt.index ) {
				_this[ _this.opt.effect ]( i );
				
				_this._prev = i;
			}
		});
		
		// Bind the buttons events
		// click, mouseover, ...
		this._btns[ this.opt.event ](function() {
			clearTimeout( _this.timer );
			clearTimeout( btnDelay );
			
			var btn = $( this ), index = this.index;
			
			if( index != _this._prev ) {
			
				btnDelay = setTimeout(function() {
					// Call cutover prototype function
					// fadeIn, show, slide
					_this[ _this.opt.effect ]( index );
					
				}, 120 );
			
			}
			
			if( !_this.opt.clickJump && _this.opt.event !== 'mouseover' ) {
				return false;
			}
		});
		
		this._btns[ 'mouseout' ](function() {
			clearTimeout( btnDelay );
		});
	},
	
	bind: function() {
		var _this = this, elem = $( this.elem );
		
		// Clear timer
		// Set the auto play
		if( !this.opt.auto ) {
			return false;
		}
		
		elem.bind({
			mouseover: function() {
				clearTimeout( _this.timer );
			},
			mouseout: function() {
				_this.timer = setTimeout(function(){ _this.autoPlay(); }, _this.opt.wait );
			}
		});
		
		this.timer = setTimeout(function(){ _this.autoPlay(); }, this.opt.wait );
	},
	
	// Bind the content to button
	bindContent: function( index ) {
		var btn = this._btns.eq( index );
		
		if( btn.attr( 'ajax' ) || btn.attr( 'lazy' ) ) {
			btn.data( 'content', false );
			
			return false;
		}
		
		if( btn.attr( 'rela' ) ) {
			btn.data( 'content', $( btn.attr( 'rela' ) ) );
			
			if( this.opt.effect !== 'slide' ) {
				btn.data( 'content' ).hide();
			}
		} else {
			btn.data( 'content', this._items.eq( index ) );
		}
	},
	
	// Get html by ajax or button name
	getLazyContent: function( index ) {
		var _this = this, btn = this._btns.eq( index );
		
		if( btn.data( 'content' ) ) {
			return false;
		}
		
		if( btn.attr( 'lazy' ) ) {
			var lazy = btn.attr( 'lazy' );
			
			this._items.eq( index ).html( lazy );
			
			btn.data( 'content', this._items.eq( index ) );
				
			this[ this.opt.effect ]( index );
		}
		
		if( btn.attr( 'ajax' ) ) {
			$.ajax({
				data: 'get',
				url	: btn.attr( 'ajax' ),
				success: function( h ) {
					_this._items.eq( index ).html( h );
					
					btn.data( 'content', _this._items.eq( index ) );
						
					_this[ _this.opt.effect ]( index );
				}
			});
		}
	},
	
	// Set the buttons style
	setBtn: function( index ) {
		this._btns.eq( this._prev ).removeClass( this.classes.curBtn );
		
		this._btns.eq( index ).addClass( this.classes.curBtn );
		
		this._prev = index;
	},
	
	// Auto play 
	autoPlay: function() {
		clearTimeout( this.timer );
		
		var _this = this, nextIndex = this._prev + 1;
		
		if( nextIndex == this._len ) {
			nextIndex = 0;
		}
			
		// Call fadeIn, show, slide
		this[ _this.opt.effect ]( nextIndex );
		
		this.timer = setTimeout(function(){ _this.autoPlay(); }, this.opt.wait );
	},
	
	// ------------------------
	// Effects
	// ------------------------
	
	// Call fadeIn from jQuery
	fadeIn: function( index ) {
		this.getLazyContent( index );
		
		if( !this._btns.eq( index ).data( 'content' ) ) {
			return false;
		}
		
		if( index != this._prev ) {
			this._btns.eq( this._prev ).data( 'content' ).fadeOut( this.opt.speed );
		}
		
		this._btns.eq( index ).data( 'content' ).fadeIn( this.opt.speed );
		
		this.setBtn( index );
	},
	
	// Call show from jQuery
	show: function( index ) {
		this.getLazyContent( index );
		
		if( !this._btns.eq( index ).data( 'content' ) ) {
			return false;
		}
		
		if( index != this._prev ) {
			this._btns.eq( this._prev ).data( 'content' ).hide();
		}
		
		this._btns.eq( index ).data( 'content' ).show();
		
		this.setBtn( index );
	},
	
	// Call animate from jQuery
	slide: function( index ) {
		var _this = this, moveConfig = {}, speed = this.opt.speed, 
		
			isLeft = this.opt.direction == 'left' ? true : false;
		
		this.getLazyContent( index );
		
		if( !this._btns.eq( index ).data( 'content' ) ) {
			return false;
		}
		
		moveConfig[ isLeft ? 'left' : 'top' ] = this._btns.eq( index ).data( 'content' ).data( 'pos' ) * -1;
		
		this._slide.animate( moveConfig , speed , 'easeout' );
		
		this.setBtn( index );
	}
};
/* 
 * END Cutover
 */

$.extend( $.easing, {
	easeout: function( x, t, b, c, d ) {
		return -c*t*t/(d*d) + 2*c*t/d + b;
	}
});

})( jQuery );