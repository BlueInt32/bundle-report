/// <reference path="jquery-1.7.1.min.js" />


function LogText(text)
{
	if (true)
	{
		console.log(text);
	}
}
function LogObject(text)
{
	if (false)
	{
		console.dir(text);
	}
}
	
function SuperBind(el, bindMethod, handler, desc)
{
	if (false)
		LogText("[" + el.selector + "] one ["+bindMethod+"] and call "+handler);
		
	if (false)
		LogText("[" + el.selector + "] one ["+bindMethod+"]: desc:"+desc);
	el.unbind(bindMethod);
	el.one(bindMethod, handler);
}

function SuperBindAlways(el, bindMethod, handler)
{
	if (false)
		LogText("[" + el.selector + "] one ["+bindMethod+"] and call "+handler);
	el.unbind(bindMethod);
	el.bind(bindMethod, handler);
}

var _EmptyGuid = "00000000-0000-0000-0000-000000000000";


function htmlEncode(value){
	return $('<div/>').text(value).html();
}

function htmlDecode(value){
	return $('<div/>').html(value).text();
}

function domify(markupCode)
{
	var domElement = $('<div/>');
	domElement.append(markupCode);
	return domElement.children(":first");
}

/* Fixed Messages Boxes */
$(function()
{
	$("body").append("<div class='fixedTopBox fixedOkMessage'></div>");
	$("body").append("<div class='fixedTopBox fixedKoMessage'></div>");
});


var closeOkMessageTimer;
var closeKoMessageTimer;
function delayCloseOkMessage()
{
	$(".fixedOkMessage").slideUp("slow");
}
function delayCloseKoMessage()
{
	$(".fixedKoMessage").slideUp("slow");
}
function hideAllAndResetTimers()
{
	$(".fixedOkMessage").slideUp(0);
	$(".fixedKoMessage").slideUp(0);
	clearTimeout(closeOkMessageTimer);
	clearTimeout(closeKoMessageTimer);
}
function okMessage(text)
{
	var okP = $("<p>" + text + "</p>");
	// "<span class='remarque'>("+ okPrefix()+")</span><span class='banane'><img src='../Content/images/common/gifs/ok/b ("+rand(26)+").gif' /></span>"
	//.css("background-image", "url('/Content/images/common/gifs/ok/b ("+rand(7)+").gif')")
	hideAllAndResetTimers();
	$(".fixedOkMessage").html(okP)	.stop(true, true).slideDown();
	closeOkMessageTimer = setTimeout("delayCloseOkMessage()",2000);
	//console.log("Msg OK " + text);
}
function koMessage(text)
{
	var koP = $("<p>"+text+"</p>");
	// "<span class='banane'><img src='../Content/images/common/gifs/ko/b ("+rand(4)+").gif' /></span>"
	hideAllAndResetTimers();
	$(".fixedKoMessage").html(koP).stop(true, true).slideDown();
	closeKoMessageTimer = setTimeout("delayCloseKoMessage()",5000);
	//console.log(">> Msg KO " + text +" <<");
}

function rand(max)
{
	var idx = Math.ceil(Math.random() * max);
	return idx;
}

var okPrefixes = new Array();
prefixesInit();
function prefixesInit()
{
	//console.log("refill");
	okPrefixes.push("Chouette ! ");
	okPrefixes.push("C'est super ! ");
	okPrefixes.push("Comment tu gères... ");
	okPrefixes.push("Haha, trop yes :-) ");
	okPrefixes.push("Joie ! ");
	okPrefixes.push("OMG trop balèze ");
	okPrefixes.push("T'es pas le dernier des branquignols ! ");
	okPrefixes.push("Le talent... à l'état brut. ");
	okPrefixes.push("Vraiment épatant ! ");
	okPrefixes.push("T'es un winner ! ©Mathieu B.");
}
Array.prototype.remove = function(from, to) {
  var rest = this.slice((to || from) + 1 || this.length);
  this.length = from < 0 ? this.length + from : from;
  return this.push.apply(this, rest);
};

function okPrefix()
{
	var baseRand = Math.random();
	var randFloat = baseRand * okPrefixes.length;
	var idx = Math.floor(randFloat);
	//console.log(idx);
	okPrefixes.remove(idx);
	if (okPrefixes.length == 0)
		prefixesInit();
	return okPrefixes[idx-1];
}

/* CODE JS */
$(function()
{
	$(".autoempty").each(function()
	{
		if($(this).val() === '') // si champ vide, y mettre la valeur de data-defaultempty
		{
			var defaultForEmptyFieldText = $(this).data("defaultempty"); 
			$(this).val(defaultForEmptyFieldText);
		}
	});
	$(".autoempty").live("focus", function(){ // si champ = data-defaultempty, le mettre à vide
		var defaultForEmptyFieldText = $(this).data("defaultempty"); 
		if($(this).val() === defaultForEmptyFieldText) 
			$(this).val("");
				
	});
	$(".autoempty").live("blur", function(){ //  si champ = vide quand on quitte le champ text, on met data-defaultempty
		var defaultForEmptyFieldText = $(this).data("defaultempty"); 
		if($(this).val() === "") 
			$(this).val(defaultForEmptyFieldText);
	});
});


/* Avant le submit, appeler la fonction qui vide tous les champs dont la valeur est égale à  data-defaultempty */
function beforeSubmit()
{
	$(".autoempty").each(function()
	{
		if($(this).val() === $(this).attr("data-defaultempty"))
			$(this).val('');
	});
}

/* CODE HTML   */
<input type="text" class="bidule autoempty" data-defaultempty="Votre n° de rue"/>
