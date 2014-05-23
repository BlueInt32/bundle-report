$(function ()
{
	// Auto empty fields Handle
	$(".autoempty").each(function ()
	{
		var defaultForEmptyFieldText = $(this).data("defaultempty");
		if ($(this).val() == "")
		{
			$(this).val(defaultForEmptyFieldText);
			if(!$(this).hasClass("norecolor"))
				$(this).css({ color: "#8D8D8D" });
		}
	});
	$(".autoempty").live("focus", function ()
	{
		var defaultForEmptyFieldText = $(this).data("defaultempty");
		if ($(this).val() == defaultForEmptyFieldText)
		{
			$(this).val("");
			$(this).css({ color: "#000000" });
		}
	});
	$(".autoempty").live("blur", function ()
	{
		var defaultForEmptyFieldText = $(this).data("defaultempty");
		if ($(this).val() == "")
		{
			$(this).val(defaultForEmptyFieldText); 
			if (!$(this).hasClass("norecolor"))
				$(this).css({ color: "#8D8D8D" });
		}
	});
});