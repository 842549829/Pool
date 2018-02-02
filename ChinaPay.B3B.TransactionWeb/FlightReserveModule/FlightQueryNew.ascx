<%@ Control Language="C#" AutoEventWireup="true" EnableViewState="false" CodeBehind="FlightQueryNew.ascx.cs" Inherits="ChinaPay.B3B.TransactionWeb.FlightReserveModule.FlightQueryNew" %>
<div id="formBox">
        <div id="hotCity" class="tipControl" style="display: none;">
        <span>热门城市(支持汉字/拼音/三字码)</span><img alt="关闭" src="/Images/ico_close.gif" width="11" height="11" />
        <div class="hotCityNav"><span class="active">热门</span><span>A-G</span><span>H-L</span><span>M-T</span><span>W-Z</span></div>
        <ul class="cityList"></ul>
        <ul class="cityList"></ul>
        <ul class="cityList"></ul>
        <ul class="cityList"></ul>
        <ul class="cityList"></ul>
    </div>
    <div id="divAirlineSelector" class="airlineSelector tipControl" style="display: none;">
        <span style="color: #F60">请选择航空公司</span><img alt="关闭" src="/Images/ico_close.gif" width="11" height="11" />
        <ul class="clearfix"></ul>
    </div>

				<div class="divType1">
					<input type="radio" id="radOneWay" name="vayageType" value="0" checked="checked"/><label for="radOneWay">单程</label>
					<br>					
                	<input type="radio" id="radRoundWay" name="vayageType" value="1" /><label for="radRoundWay">往返</label>
                    <input type="hidden" id="txtAirlineValue"/>
				</div>
				<div class="divType2">
					<div class="boxTitle name">出发：</div>		
					<div class="borderBox">
                    	<input type="hidden" id="txtDepartureValue"/>	
						<input type="text" id="txtDeparture" class="inputBox" autocomplete="false"
                         onblur="airportControlBlured($('#txtDepartureValue'),$('#hotCity'),$(this));"
                    onkeyup="showCitis($('#txtDepartureValue'),$('#hotCity'),$(this),event);" 
                    onfocus="showCitis($('#txtDepartureValue'),$('#hotCity'),$(this),event);$(this).select();"/>
						
                        <span onclick="showCitiesSelectWindow($('#txtDepartureValue'),$('#hotCity'),$('#txtDeparture'));" class="cityBox" title="选择出发城市"></span>
					</div>
					<div onclick="change()" id="changeCity" style="cursor:pointer">&nbsp;</div>
					<br>
					<div class="boxTitle">到达：</div>				
					<div class="borderBox">
					    <input type="hidden" id="txtArrivalValue"/>
						<input type="text" id="txtArrival" class="inputBox" autocomplete="false" onblur="airportControlBlured($('#txtArrivalValue'),$('#hotCity'),$(this));"
                    onkeyup="showCitis($('#txtArrivalValue'),$('#hotCity'),$(this),event);" 
                    onfocus="showCitis($('#txtArrivalValue'),$('#hotCity'),$(this),event);$(this).select();"/>
						<span onclick="showCitiesSelectWindow($('#txtArrivalValue'),$('#hotCity'),$('#txtArrival'));" class="cityBox"  onclick="showCitiesSelectWindow($('#txtArrivalValue'),$('#hotCity'),$('#txtArrival'));" ></span>
					</div>
				</div>
				<div class="divType2">
					<div class="boxTitle">出发日期：</div>			
					<div class="borderBox">
						<input type="text" id="txtGoDate" class="inputBox" onfocus="WdatePicker({isShowClear:false,readOnly:true,minDate:'%y-%M-%d',doubleCalendar:true});"/>
						<span onclick="WdatePicker({isShowClear:false,el:'txtGoDate',readOnly:true,minDate:'%y-%M-%d',doubleCalendar:true});"  class="arrivalBox"></span> 
					</div>
					<br>
					<div class="boxTitle gobackdate">返程日期：</div>
					<div class="borderBox gobackdate">
						 <input type="text" id="txtBackDate" class="inputBox" onfocus="WdatePicker({isShowClear:false,readOnly:true,minDate:'#F{$dp.$D(\'txtGoDate\')}',doubleCalendar:true})"/>
						<span onclick="WdatePicker({isShowClear:false,el:'txtBackDate',readOnly:true,minDate:'#F{$dp.$D(\'txtGoDate\')}',doubleCalendar:true})" class="returnBox"></span> 
					</div>
				</div>
				<div class="divType1">
					 <button type="button" id="btnQueryFlight">航班查询</button>
				</div>
				<a id="inquireHistory" class="obvious-a" style="text-decoration: none;">查询历史：</a>
			<ul id="recentlyHistory">
			</ul>
		</div>