<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Bunk_new.aspx.cs" Inherits="ChinaPay.B3B.MaintenanceWeb.BasicData.Bunk_new" EnableSessionState="ReadOnly" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="x-ua-compatible" content="ie=7" />
    <!--让IE8解释和IE7一样，不可删-->
    <title>舱位代码维护</title>
    <link rel="stylesheet" type="text/css" href="../css/style.css" />
    <style type="text/css">
        .contents
        {
            height: 500px;
        }
        dd span
        {
            color: #333;
            font-weight: 400;
        }
</style>
</head>
<body>
    <form id="Form1" runat="server">
    <div class="contents">
        <div class="breadcrumbs">
            <span>当前位置:</span><span>基础数据管理</span>&raquo;<span>舱位折扣维护</span>
        </div>
        <div class="title">
            <dl>
                <dd class="searchbk">
                    <table width="97%" cellspacing="0" cellpadding="0" border="0" class="search">
                        <colgroup>
                            <col style="width:100px;" />
                            <col style="width:400px;" />
                            <col />
                        </colgroup>
                        <tbody>
                            <tr>
                                <th>
                                    航空公司：
                                </th>
                                <td colspan="5">
                                   <asp:DropDownList ID="ddlAirline" CssClass="input2" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    航班开始日期：
                                </th>
                                <td>
                                    <asp:TextBox ID="txtHBStartDate" runat="server" CssClass="input2" onfocus="WdatePicker({isShowClear:true,readOnly:true,maxDate:'#F{$dp.$D(\'txtHBStopDate\')}'})"></asp:TextBox>
                                </td>
                                <th>
                                    航班截止日期:
                                </th>
                                <td>
                                    <asp:TextBox ID="txtHBStopDate" runat="server" CssClass="input2" onfocus="WdatePicker({isShowClear:true,readOnly:true,minDate:'#F{$dp.$D(\'txtHBStartDate\')}'})"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <th>出票日期：</th>
                                <td colspan="3">
                                    <asp:TextBox ID="txtCpStartDate" runat="server" CssClass="input2" onfocus="WdatePicker({isShowClear:true,readOnly:true,maxDate:'#F{$dp.$D(\'txtHBStartDate\')}'})"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="city_new">
                            <th>
                                出发机场：
                            </th>
                            <td>
                                <asp:DropDownList ID="ddlDepartCity" CssClass="input2" runat="server"  disabled="false" ></asp:DropDownList>
                            </td>
                            <th>
                                到达机场：
                            </th>
                            <td colspan="3">
                                <asp:DropDownList ID="ddlArriveCity" CssClass="input2" runat="server"  disabled="false" ></asp:DropDownList>
                            </td>
                            </tr>
                            <tr>
                                <th>
                              舱位类型：
                                </th>
                                <td class="gl2">
                                    <asp:RadioButtonList ID="radiolist" runat="server" 
                                        RepeatDirection="Horizontal" RepeatLayout="Flow" RepeatColumns="5"  Font-Bold="false"  CssClass="fontColor01">
                                    </asp:RadioButtonList>
                                 </td>   
                                <th class="cType">
                                <span class="sptejia fontColor01" > 舱位描述：</span>
                                <span class="fontColor01 dropMpType" > 舱位描述：</span>
                                <span class="sptoudeng" style="display:none" > 头等/公务类型：</span>
                                </th>
                                 <td class="cType">
                                          <span class="sptejia">
                                             <asp:DropDownList ID="ddlTJType" CssClass="input2"  runat="server" Width="100px">
                                            </asp:DropDownList>
                                        </span>
                                         <span class="sptoudeng"  style="display:none;">
                                             <asp:DropDownList ID="ddltdType" CssClass="input2"  runat="server" Width="100px">
                                            </asp:DropDownList>
                                        </span>
                                        <span class="dropMpType" style="display:none;">
                                            <asp:DropDownList ID="dropMpType" runat="server" CssClass="input2" Width="100px">
                                            </asp:DropDownList>
                                        </span>
                                 </td>
                            </tr>
                            <tr>
                                <th>适用行程</th>
                                <td colspan="3">
                                    <asp:CheckBoxList runat="server" ID="chklVoyageType" RepeatDirection="Horizontal" RepeatLayout="Flow" Font-Bold="false"  CssClass="fontColor01"></asp:CheckBoxList>
                                </td>
                            </tr>
                            <tr>
                                <th>适用旅行类型</th>
                                <td>
                                    <asp:CheckBoxList runat="server" ID="chklTravelType"  RepeatDirection="Horizontal" RepeatLayout="Flow" Font-Bold="false"  CssClass="fontColor01"></asp:CheckBoxList>
                                </td>
                                <th>适用旅客类型</th>
                                <td>
                                    <asp:CheckBoxList runat="server" ID="chklPassengerType"  RepeatDirection="Horizontal" RepeatLayout="Flow" Font-Bold="false"  CssClass="fontColor01"></asp:CheckBoxList>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    舱位代码：
                                </th>
                                <td class="gl">
                                    <asp:TextBox ID="txtCwCode" runat="server" CssClass="input2" bitian="1" dataType="SafeString" showname="舱位代码"></asp:TextBox>
                                    <span class="add">
                                        <img src="../images/Add.png" alt="" /></span><span class="cut"><img src="../images/Cut.png" alt="" /></span>
                                </td>
                                <th height="23" class="yc">
                                    折扣：
                                </th>
                                <td class="yc">
                                    <asp:TextBox ID="txtDiscount" runat="server" CssClass="input2" bitian="1" dataType="SafeString" showname="折扣"></asp:TextBox>
                                </td>
                            </tr>
                            <tr class="tr" id="trFirst" style="display:none;"><td colspan="5"></td></tr>
                            <tr>
                                <th>
                                    退票规定：<br />
                                    更改规定：<br />
                                    签转规定：<br />
                                    备注:
                                </th>
                                <td colspan="5">
                                    <asp:TextBox ID="txtRefundRegulation" runat="server" CssClass="input1" style="width:100%;"></asp:TextBox><br />
                                    <asp:TextBox ID="txtChangeRegulation" runat="server" CssClass="input1" style="width:100%;"></asp:TextBox><br />
                                    <asp:TextBox ID="txtEndorseRegulation" runat="server" CssClass="input1" style="width:100%;"></asp:TextBox><br />
                                    <asp:TextBox ID="txtRemarks" runat="server" CssClass="input1" style="width:100%;"></asp:TextBox><br />
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    舱位状态：
                                </th>
                                <td colspan="5">
                                    <asp:RadioButtonList ID="radseatlist" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" Font-Bold="false"  CssClass="fontColor01"> 
                                     <asp:ListItem Value="T" Selected="True">启用</asp:ListItem>
                                        <asp:ListItem Value="F">禁用</asp:ListItem>
                                    </asp:RadioButtonList>
                                    

                               </td>
                            </tr>
                            <tr class="operator">
                                <td colspan="6">
                                    <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="button"  
                                        OnClientClick="return btnSubmit()" onclick="btnSave_Click"/>&nbsp;&nbsp;
                                    <input type="button" class="button" value="返回" name="button" onclick="javascript:window.location.href='Bunk.aspx?Search=Back'" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </dd>
            </dl>
        </div>
    </div>
    <div class="clear">
    </div>
     <input  type="hidden" id="hiddindex" value="0" runat="server" /><!--添加时保存子舱位的行数-->
     <input  type="hidden" id="seatName" value="" runat="server" /><!--保存扩展舱位名称，折扣分别用‘，分割’ 多个用‘|’分割-->
     <input type="hidden" id="hiddtr" value="" runat="server" /><!--修改时保存子舱位的行数-->
     <input type="hidden" id="hiddUpdate" value="0" runat="server" /><!--当前是修改-->
     <input type="hidden" id="seatTJ" value="" runat="server" /><!--特价子舱位-->
    </form>
</body>
</html>
<script type="text/javascript" language="javascript" src="../js/jquery.js"></script>
<script src="../js/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
<script src="/Scripts/BasicData/Bunk_new.js?version=1.1" type="text/javascript"></script>
