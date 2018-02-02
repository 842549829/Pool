(function( $ ) {
	
$.fn.tabs = function( option ) {
	
	var _this = this, prev = null, list = $( 'a', this );
	
	// Default options
	var opt = {
		index		: 0,
		event		: 'click',
		selected	: 'ep_selected',
		callback	: function() {}
	};
	
	$.extend( opt, option );
	
	// Default settings
	list.each(function( i ) {
		var sr = href = $( this ).attr( 'href' );
		
		this._isAjax = false;
		
		this._index = i;
		
		if( /^#/.test( href ) ) {
		
			if( opt.index === i ) {
				$( this ).addClass( opt.selected );
				
				prev = this;
			} else {
				$( sr ).hide();
			}
			
			this._content = $( sr );
		} 
		
		// Ajax
		else {
			// Show ajax content first
			if( opt.index === i ) {
				ajaxContent( this );
		
				prev = this;
			} else {
				this._isAjax = true;
			}
		}
		
		// Callback function
		//opt.callback.call( this, i );
	});
	
	// Link events
	list.live( opt.event, function() {
		if( prev._index === this._index ) {
			return false;
		}
		
		if( !this._isAjax ) {
			// Display previous content
			this._content.show();
		} 
		
		// Ajax get content and append
		else {
			ajaxContent( this );
		}
			
		$( this ).addClass( opt.selected );
		
		// Update menu class
		$( prev ).removeClass( opt.selected );
		
		// Hide previous content
		prev._content.hide();
		
		// Callback function
		opt.callback.call( this, this._index );
		
		prev = this;
	});
	
	list.live( 'click', function() {
		return false;
	});
	
	function ajaxContent( elem ) {
		var cid = 'ep_tabs_' + ( new Date() ).getTime();
		
		// Append the content
		_this.after( 
			elem._content = $( '<div id="'+ cid +'"></div>' ).load( $( elem ).attr( 'href' ) )
		);
		
		// Reset
		elem._isAjax = false;
	}
	
};
})( jQuery );