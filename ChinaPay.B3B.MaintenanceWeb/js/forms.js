    //表单控制 全选
	$(function(){
        $("#CheckedAll").click(function(){
			$('[name=che]:checkbox').attr("checked",this.checked);
		}); 
    });
    $(function(){
        $('[name=che]:checkbox').click(function(){
			var $flag = true;
		    $('[name=che]:checkbox').each(function(){
				if(!this.checked){
					flag = false;
					}
			});
			$("#CheckedAll").attr('checked',flag)
		});      
    });