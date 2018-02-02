/*----------------------------------------------------------------
    文件名：        DateExtensions.js
    文件功能描述：  扩展 JavaScript 中针对日期对象的操作。

    创建标识：      Izual20100122
----------------------------------------------------------------*/
(function(){
    Date.weeks=['Sun','Mon','Tues','Wed','Thur','Fri','Sat'];
    Date.fullWeeks=['Sunday','Monday','Tuesday','Wednesday ','Thursday','Friday','Saturday'];
    Date.chineseWeeks=['星期日','星期一','星期二','星期三','星期四','星期五','星期六'];
    Date.isDate=function(obj){
    ///<summary>判断给定参数是否日期类型。</summary>
    ///<param name="obj">要判断的对象。</param>
    ///<returns>如参数 obj 是日期类型，返回 true，否则返回 false.</returns>
        return obj && obj instanceof Date;
    }
    Date.addPart=function(date,part,n){
    ///<summary>修改日期中指定部分。</summary>
    ///<param name="date">要修改的日期对象。</param>
    ///<param name="part">字符串，表示要修改的日期部分。区分大小写。 ('y':年;'M':月;'d':日;'h':时;'m':分;'s':秒;'f':毫秒)</param>
    ///<param name="n">整型，表示修改的量。</param>
    ///<returns>返回修改后的新日期。</returns>
        if(date instanceof Date){
            var parts = {
                'y':date.getFullYear(),
                'M':date.getMonth(),
                'd':date.getDate(),
                'h':date.getHours(),
                'm':date.getMinutes(),
                's':date.getSeconds(),
                'f':date.getMilliseconds()
            }

            if(part in parts){
                n = typeof n == 'number' ? Math.floor(n) : 0;
                parts[part] += n;
                return new Date(parts.y,parts.M,parts.d,parts.h,parts.m,parts.s,parts.f);
            }
        }
    };
    Date.prototype.addPart=function(part,n){
    ///<summary>修改日期中指定部分。</summary>
    ///<param name="part">字符串，表示要修改的日期部分。区分大小写。 ('y':年;'M':月;'d':日;'h':时;'m':分;'s':秒;'f':毫秒)</param>
    ///<param name="n">整型，表示修改的量。</param>
    ///<returns>返回修改后的新日期。</returns>
        return Date.addPart(this,part,n);
    };
    
    
    Date.isLeapYear = function(year){
    ///<summary>判断指定的年份是否闰年。</summary>
    ///<param name="year">要判断的年份。</param>
    ///<returns>如果当前日期中的年份是闰年返回 true,否则返回 false.</returns>
        if(typeof year == 'number' && year > 0){
            if ((year % 4) != 0){
                return false;
            }
            if ((year % 100) == 0){
                return ((year % 400) == 0);
            }
            return true;
        }
    };
    Date.prototype.isLeapYear = function(){
    ///<summary>判断当前日期的年份是否闰年。</summary>
    ///<returns>如果当前日期中的年份是闰年返回 true,否则返回 false<./returns>
        return Date.isLeapYear(this.getFullYear());
    };
    
    Date.getDays=function(year,month){
    ///<summary>获取指定年份中某月的天数。</summary>
    ///<param name="year">年份</param>
    ///<param name="month">月份</param>
    ///<returns>返回获取到的天数。</returns>
        if(typeof year == 'number' && typeof month == 'number' && year > 0 && month > 0 && month < 13){
            if(month==2){
                return Date.isLeapYear(year) ? 29 : 28;
            }
            if(month % 2 != 0){
                return month > 7 ? 30 : 31;
            }
            else{
                return month > 6 ? 31 : 30;
            }
        }
    };
    Date.prototype.getDays=function(){
    ///<summary>获取当前月份的天数。</summary>
    ///<returns>返回获取到的天数。</returns>
        return Date.getDays(this.getFullYear(),this.getMonth()+1);
    };

    Date.format=function(date,fmt){
    ///<summary>格式化日期。</summary>
    ///<param name="date">要格式化的日期对象。</param>
    ///<param name="fmt">格式字符串，区分大小写。('y':年;'M':月;'d':日;'h':时;'m':分;'s':秒;'f':毫秒)</param>
    ///<returns>返回格式化后的字符串。</returns>
        if(date instanceof Date){
            if(typeof fmt != 'string' || fmt == ''){return date.toLocaleString();}
            var parts = {
                'M+' : date.getMonth()+1,
                'd+' : date.getDate(),
                'h+' : date.getHours(),
                'm+' : date.getMinutes(),
                's+' : date.getSeconds(),
                'f+' : date.getMilliseconds(),
                'w'  : Date.weeks[date.getDay()],
                'W'  : Date.fullWeeks[date.getDay()]
            }
            if(/(y+)/.test(fmt)){
                fmt=fmt.replace(RegExp.$1,(date.getFullYear()+"").substr(4 - RegExp.$1.length));
            }
            for(var part in parts){
                if(new RegExp("("+ part +")").test(fmt)){
                    if(part == 'w' || part == 'W'){
                        fmt = fmt.replace(RegExp.$1,parts[part]);
                    }
                    else{
                        fmt = fmt.replace(RegExp.$1,RegExp.$1.length==1 ? parts[part] : ("00"+ parts[part]).substr((""+ parts[part]).length));
                    }
                }
            }
            return fmt;
        }
    };

    Date.prototype.format=function(fmt){
    ///<summary>格式化日期。</summary>
    ///<param name="fmt">日期格式</param>
    ///<returns>返回格式化后的字符串。</returns>
        return Date.format(this,fmt);
    };

    Date.fromString=function(str){
    ///<summary>将指定的字符串转换为对应的日期。</summary>
    ///<param name="str">要转换成日期的字符串。</param>
    ///<returns>成功返回与参数字符串对应的日期，否则返回 null</returns>
        if(/(\d{2,4})\-(\d{1,2})\-(\d{1,2})/.test(str)){
            var year=+RegExp.$1;
            var month=+RegExp.$2;
            var day=+RegExp.$3;
            if(year<100){
                if(year<50){
                    year+=2000;
                }
                else{
                    year+=1900;
                }
            }
            if(month>12){
                return null;
            }
            if(day>Date.getDays(year,month)){
                return null;
            }
            return new Date(year,month-1,day);
        }
        return null;
    };
    
    Date.diff=function(d1,d2,t){
    ///<summary>获取两个日期的差值。</summary>
    ///<param name="d1">用于计算差值的第一个日期</param>
    ///<param name="d2">用于计算差值的第二个日期</param>
    ///<param name="t">返回的差值类型。('w':星期;'d':日;'h':时;'m':分;'s':秒,如果 type 不是其中任何一种类型，返回两个日期相差的毫秒数)</param>
    ///<returns>根据参数 t 提供的类型返回 d1-d2 的差值。</returns>
        var types = {
            's' : 1000,
            'm' : 60*1000,
            'h' : 60*1000*60,
            'd' : 24*60*1000*60,
            'w' : 7*24*60*1000*60
        };
        if(t in types){
            return (d1-d2)/types[t];
        }
        else{
            return d1-d2;
        }
    };
    
    Date.prototype.diff=function(date,t){
    ///<summary>获取指定日期与当前日期的差值。</summary>
    ///<param name="d">用于计算差值的日期</param>
    ///<param name="t">返回的差值类型。('w':星期;'d':日;'h':时;'m':分;'s':秒,如果 type 不是其中任何一种类型，返回两个日期相差的毫秒数)</param>
        return Date.diff(this,date,t);
    };
})();