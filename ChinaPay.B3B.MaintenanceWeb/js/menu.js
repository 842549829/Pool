function liStyle( index )
{
	for(var i = 1 ; i <= 6 ; i++ )
	{
		//默认选中当前页的li
		var li = document.getElementById("li_"+i);
		if(i == index)
		{
			li.className = "over";			
		}
		else
		{
			li.className = "";
		}
		//如果li样式不是over，就给鼠标滑过事件赋值
		if(li.className != "over")
		{
			li.onmouseover = function(){this.className='over'};
			li.onmouseout = function(){this.className='out'};
		}
		
	}
}