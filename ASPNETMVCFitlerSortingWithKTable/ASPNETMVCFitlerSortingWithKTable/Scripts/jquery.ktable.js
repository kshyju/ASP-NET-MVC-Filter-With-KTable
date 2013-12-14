/*
*   kTable plugin for HTML tables,  
*   Free to use under the MIT license.
*   Original Author : Shyju (Twitter : @kshyju)

*/
; (function ($, window, document, undefined) {
    if (!$.techiesweb) {
        $.techiesweb = {};
    };
    $.fn.ktable = function (options) {
        //default options
        var settings = {
            headerCheckBoxClass: "chkAll",
            checkedRowClass: "rowSelected",
            singleRowCheckBoxClass: "chkSelect",
            rowHighlightClass: "rowHover",
            headerClass: "tHeader",
            filterable: {
                filterHeaderText: "Show me items with value:",
                filterAndText: "And",
                filterableColumnClassName: 'filterable',
                callBackOnFilter: '',
                callBackOnClear: '',
                bindDefaultDatePicker: false,
                dateFormat: "mm/dd/yy",
                validationErrorClassName:'ktable-filter-validation-error'
            },
            sortable:
            {
              callBackOnAscending :'',
              callBackOnDescending:''

            }
        };

        $.extend(true,settings,options);      


       return this.each(function () {
            var _tbl = $(this);
           
           //Header row CSS class
            _tbl.find("th").addClass(settings.headerClass);

            // for check all
          _tbl.find("input[type='checkbox']." + settings.headerCheckBoxClass).bind("click", function () {
                if ($(this).is(":checked")) {
                    _tbl.find("input[type='checkbox']." + settings.singleRowCheckBoxClass).prop('checked', true).closest("tr").addClass(settings.checkedRowClass);
                }
                else {
                    _tbl.find("input[type='checkbox']." + settings.singleRowCheckBoxClass).prop('checked', false).closest("tr").removeClass(settings.checkedRowClass);
                }

            });

            //for single row
            _tbl.find("input[type='checkbox']." + settings.singleRowCheckBoxClass).bind("click", function () {
                if ($(this).is(":checked")) {
                    $(this).closest("tr").addClass(settings.checkedRowClass);
                }
                else {
                    $(this).closest("tr").removeClass(settings.checkedRowClass);
                }
            });

            //Hover effect for rows
           _tbl.find("tr").each(function (index,item) {
                $(item).hover(
                    function(){  $(this).addClass(settings.rowHighlightClass); },
                    function(){  $(this).removeClass(settings.rowHighlightClass); }   
                );
            });
                     
   //Filter
          if (settings.filterable.callBackOnFilter)
          {

           var tableHeaders = _tbl.find("th." + settings.filterable.filterableColumnClassName);
           tableHeaders.each(function (index, item) {
               var _header = $(item);
               var headerText = _header.text();
               var headerId=_header.attr("id");


               var dataType = "string";
               if (_header.attr("datatype") !== undefined)
                   dataType = _header.attr("datatype");

               headerText = $.trim(headerText);
              
               var anchorId = headerText.split('/').join('').split(' ').join('');
              
               if(typeof headerId=='undefined')
                headerId=anchorId;

               


               var newHeaderMarkup = "<a href='#' datatype='" + dataType + "' id='" + headerId + "' class='ktable-filter'><span class='k-filter'></span></a>";
               if(settings.sortable.callBackOnAscending)
               {

                newHeaderMarkup+="<a class='ktable-sortHeader' id='sort-"+headerId+"' for='"+headerId+"'>" + headerText + "<span class='ktable-sort' sortorder='none'></span></a>";
               }
              else

              {
               newHeaderMarkup+="<a>" + headerText + "</a>";
             }
               _header.html(newHeaderMarkup);

           });

           //event binding for the dynamically added elements




           $(document).on("click", "a.ktable-filter", function (e) {           

               //build the animation container
               var _clickedAnchor = $(this);
               clickedAnchorId = _clickedAnchor.attr("id");
               var popupId = "popup-" + clickedAnchorId;              

               var dataType = "string";
               if (_clickedAnchor.attr("datatype") === "date")
                   dataType = "date";


               var headerHeight = _clickedAnchor.closest("tr").height();
               var top = _clickedAnchor.closest("tr").offset().top + headerHeight;
               var left = e.pageX - _clickedAnchor.closest(".k-grid-filter").width();

               

                var popupWidth=$("#"+popupId).width();
               
               

               //Hide existing open popups, remove the selected filter css class
               $(".popupContainer").slideUp(200);
               $("span.filter-selected").each(function () {
                   if ($(this).hasClass("active")==false) {
                       $(this).removeClass("filter-selected");
                   }
               });

               if ($("#" + popupId).length > 0) {
                   $("#" + popupId).fadeIn(100);
                   _clickedAnchor.find("span.k-filter").addClass("filter-selected"); 
                   return;
               }           
               
               var popupMarkup = "<div id='" + popupId + "' for='" + clickedAnchorId + "' class='popupContainer' style='position:absolute;display:none;'>";
               popupMarkup += "<form class='ktableform' for='" + clickedAnchorId + "' data-type='" + dataType + "'>";
               popupMarkup+="<div>";
               popupMarkup += "<div class='filterHeader'>" + settings.filterable.filterHeaderText + "</div>";
               popupMarkup += "<div class='searchType'><select class='ktable-searchtype'>" + getSelectOptions (_clickedAnchor)+ "</select></div>";
               if (dataType === "date") {
                 
                   popupMarkup += "<div class='searchBox'><input type='text' class='filter-input kTableDatePicker active-filter-input' /></div>";
                   popupMarkup += "<div class='divBetween' style='display:none;'>" + settings.filterable.filterAndText + "<br/><div class='searchBox'><input type='text' class='filter-input kTableDatePicker' /></div></div>";
               }
               else {                 
                   popupMarkup += "<div class='searchBox'><input type='text' class='filter-input active-filter-input' /></div>";
               }

               popupMarkup += "<div><button class='filter-button' type='submit'>Filter</button><button class='filter-button' type='reset'>Close</button></div>";
               popupMarkup += "</div></form>";
               popupMarkup += "</div>";


               $(document.body).append(popupMarkup);

               // Set the position now

                var windowWidth=$(window).width(); 
              //  console.log("windowWidth : "+windowWidth);
               // console.log("left : "+left+" , width : "+windowWidth);
                 var popupWidth=$("#"+popupId).width();
                //console.log("popUpWidth:"+popupWidth);
                
                var rightPos=windowWidth-left;

                var position;
                if(left+popupWidth<windowWidth)
                {
                    // OK to show in regular place
                  position= { top: top, left:left};
                }
                else
                { 
                  // Use the right side positioning
                  position= { top: top, right:rightPos};

                }
               $("#" + popupId).css(position).slideDown(300);



               //highlight the clicked filter icon
               _clickedAnchor.find("span.k-filter").addClass("filter-selected");

               //Bind date picker to the ui
               
               if (settings.filterable.bindDefaultDatePicker) {                               
                   if (jQuery.ui) {
                       {
                           $("input.kTableDatePicker").datepicker({ dateFormat: settings.filterable.dateFormat });
                       }
                   }
               }

           });

           //Execute the callback function when clicking on the filter button 
           $(document).on("click", "button[type='submit']", function (e) {
               e.preventDefault();
               var isSearchFormEmpty = false;
               var _clickedSubmit = $(this);
               var anchorId = _clickedSubmit.closest("div.popupContainer").attr("for");

               //LEts clear the existing validation css on inputs
               $("input." + settings.filterable.validationErrorClassName).removeClass(settings.filterable.validationErrorClassName);
               //Validate the seach for inputs
               var inputs = _clickedSubmit.closest("div.popupContainer").find("input.active-filter-input");
               $.each(inputs, function (e) {                  
                   if ($(this).val() == "") {
                       isSearchFormEmpty = true;
                       $(this).addClass(settings.filterable.validationErrorClassName).focus();
                       return;
                   }
               });
               if (isSearchFormEmpty == false) {
                   $("a#" + anchorId).find("span").addClass("active");

                   if (settings.filterable.callBackOnFilter)
                       settings.filterable.callBackOnFilter(_clickedSubmit.closest("form"));

                   _clickedSubmit.closest("div.popupContainer").hide();
               }
           });

           //Close the popup when clicking
           $(document).on("click", "button[type='reset']", function (e) {
               e.preventDefault();
               var _clickedReset = $(this);
               var anchorId = _clickedReset.closest("div.popupContainer").attr("for");
               _clickedReset.closest("div.popupContainer").find("input.filter-input").val("");
               _clickedReset.closest("div.popupContainer").hide();
               $("#" + anchorId).find("span.k-filter").removeClass("filter-selected");

               if (settings.filterable.callBackOnClear)
                   settings.filterable.callBackOnClear(_clickedReset.closest("form"));
           });

              //Change event on the ddlSearchType SELECT 
              //If user selected between option, show the end date textbox too;
           $(document).on("click", "SELECT.ktable-searchtype", function (e) {
               var val = $(this).val();
               if (val == "between") {
                   $("div.divBetween").find("input").addClass("active-filter-input");
                   $("div.divBetween").fadeIn(100);
               }
               else
                   $("div.divBetween").fadeOut(100).find("input").val("");
           });


           // Sort events

          $(document).on("click", "a.ktable-sortHeader", function (e) {          
            var _clickedAnchor = $(this);
           
            //Remove existing sorts css class and attributes on other columns
            $("a.ktable-sortHeader").each(function(){
              var _thisSort=$(this);
              if(_thisSort.attr("id")!=_clickedAnchor.attr("id"))
              {                 
                _thisSort.removeClass("sort-active").find("span.ktable-sort").removeClass("ascsort descsort").attr("sortorder","none");
              }
            });
           

            var currentColumnSortSpan=_clickedAnchor.find("span.ktable-sort");
            var currentSortOrder=currentColumnSortSpan.attr("sortorder");

            if(currentSortOrder==="none" || currentSortOrder=="desc")
            {               
              currentColumnSortSpan.attr("sortorder","asc").removeClass("descsort").addClass("ascsort");
            }
            else
            {
              currentColumnSortSpan.attr("sortorder","desc").removeClass("ascsort").addClass("descsort");
            }            

            _clickedAnchor.addClass("sort-active");

             if (settings.sortable.callBackOnAscending)
                settings.sortable.callBackOnAscending(_clickedAnchor);
          });
           //sort events



          } //if filter is enabled


//Sortable

//Sortable ends


       }); // return this.each

     
       function getSelectOptions(_clickedAnchor) {           
           if (typeof settings.filterable.dropDownContent !== 'undefined') {            
               var dropDownID = "";               
               if (_clickedAnchor.attr("datatype") === "date") {
                   dropDownID = settings.filterable.dropDownContent.source.date;
               }
               else {
                   dropDownID = settings.filterable.dropDownContent.source.text;
               }
              
               var strOptions = "";
               $("#" + dropDownID + " option").each(function () {                  
                   strOptions += "<option value='" + $(this).val() + "'>" + $(this).text() + "</option>";
               });
               return strOptions;
           }
           else {
               if (_clickedAnchor.attr("datatype") !== "date") {
                   return "<option>Contains</option><option>Starts with</option><option>Ends with</option>"
               }
               else {
                   return "<option>Equal to</option><option>Before</option><option>After</option>";
               }
           }
        };


    };      // plugin ends

})(jQuery, window, document);


