        function ShowReashoInput() {
            $(".fixed,.layers").show();
            return false;
        }
        function CancleInput() {
            $(".fixed,.layers").hide();
        }
        function CheckReason() {
            var reason = $.trim($("#Reason").val());
            if (reason=="") {
                alert("请输入拒绝原因");
                $("#Reason").select();
                return false;
            } else if(reason.length>200){
                alert("拒绝原因不能超过200字");
                return false;
            }
            return true;
        }

        $("#setTrigger").click(function ()
            {
            $("#modifyServiceCharge").show();
            $("#setTrigger").hide();
        });
        $("#save").click(function ()
        {
            var newFee = $.trim($("#newCharge").val());
            if (newFee == "")
            {
                alert("请先输入手续费！");
                return;
            }
            if (!/\d{1,6}(\.\d+)?/.test(newFee))
            {
                alert("服务费格式错误！");
                return;
            }
            $("#ChangedServiceCharge").val($("#newCharge").val());
            $("#renderedServiceCharge").text($("#newCharge").val());
            $("#totalServiceCharge").text(Round(parseFloat($("#newCharge").val()) * passengerCount, 2));
            $("#modifyServiceCharge").hide();
            $("#setTrigger").show();
        });
        var defaultText = "请在此输入修改服务费的理由或与采购和供应（或产品方）协商过程中的一些简要记录备注，以备后期查询";
        $("#ChangeServiceChargeReason").focus(function () {
            $(this).css("color", "#000");
            var currentControl = $(this);
            if (currentControl.val() == defaultText) {
                currentControl.val("");
            }
        }).blur(function () {
            var currentControl = $(this);
            if (currentControl.val() == "") {
                $(this).css("color", "#999");
                currentControl.val(defaultText);
            }

        });

        $(function ()
        {
            //passengerCount = $("#passengers_divPassengers tr").size() - 1;
            $("#totalServiceCharge").text(Round(parseFloat($("#ChangedServiceCharge").val()) * passengerCount, 2));
        });

        function CheckPriceInput() {
            if ($("#newCharge").is(":visible")) {
                var newFee = $.trim($("#newCharge").val());
                if (newFee == "")
                {
                    alert("请先输入手续费！");
                    return false;
                }
                if (!/\d{1,6}(\.\d+)?/.test(newFee))
                {
                    alert("服务费格式错误！");
                    return false;
                }
                $("#save").click();
            }
            if ($("#ChangeServiceChargeReason").val() == defaultText) $("#ChangeServiceChargeReason").val("");
            return true;
        }