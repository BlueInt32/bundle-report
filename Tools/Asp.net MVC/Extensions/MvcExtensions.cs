#region Usings

using System;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

#endregion

namespace Tools.Extensions
{
	public static class MvcExtensions
	{
		public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression)
		{
			ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
			IEnumerable<TEnum> values = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
			// 

			IEnumerable<SelectListItem> items =
				values.Select(value => new SelectListItem
				{
					Text = value.ToString(),
					Value = value.ToString(),
					Selected = value.Equals(metadata.Model)
				});

			return htmlHelper.DropDownListFor(
				expression,
				items
				);
		}
		public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, Dictionary<string, int> enumDescriptor, object htmlAttributes)
		{
			ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);

			// 

			IEnumerable<SelectListItem> items =
				enumDescriptor.Select(value => new SelectListItem
				{
					Text = value.Key,
					Value = value.Value.ToString(),
					Selected = value.Value.Equals(metadata.Model)
				});

			return htmlHelper.DropDownListFor(
				expression,
				items,
				htmlAttributes
				);
		}

		public static MvcHtmlString LimitedSizeTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
		{
			var member = expression.Body as MemberExpression;
			var stringLength = member.Member
				.GetCustomAttributes(typeof(StringLengthAttribute), false)
				.FirstOrDefault() as StringLengthAttribute;
			var attributes = (IDictionary<string, object>)new RouteValueDictionary(htmlAttributes);
			if (stringLength != null)
			{
				attributes.Add("maxlength", stringLength.MaximumLength);
			}
			return htmlHelper.TextBoxFor(expression, attributes);
		}

		public static MvcHtmlString RadioButtonForSelectList<TModel, TProperty>(
			this HtmlHelper<TModel> htmlHelper,
			Expression<Func<TModel, TProperty>> expression,
			IEnumerable<SelectListItem> listOfValues)
		{
			var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
			var sb = new StringBuilder();

			if (listOfValues != null)
			{
				// Create a radio button for each item in the list 
				foreach (SelectListItem item in listOfValues)
				{
					// Generate an id to be given to the radio button field 
					var id = string.Format("{0}_{1}", metaData.PropertyName, item.Value);

					// Create and populate a radio button using the existing html helpers 
					var label = htmlHelper.Label(id, item.Text);
					var radio = htmlHelper.RadioButtonFor(expression, item.Selected, new { id = id }).ToHtmlString();

					// Create the html string that will be returned to the client 
					// e.g. <input data-val="true" data-val-required="You must select an option" id="TestRadio_1" name="TestRadio" type="radio" value="1" /><label for="TestRadio_1">Line1</label> 
					sb.AppendFormat("<div class=\"RadioButton\">{0}{1}<div class='clear'></div></div>", radio, label);
				}
			}

			return MvcHtmlString.Create(sb.ToString());
		}

		public static string MyValidationSummary(this HtmlHelper helper, bool onlyFirst)
		{
			StringBuilder retVal  = new StringBuilder();
			if (helper.ViewData.ModelState.IsValid)
				return "";
			retVal.Append("<div class='mauvaise-reponse'>");
			int count = 0;
			foreach (var key in helper.ViewData.ModelState.Keys)
			{
				foreach (var err in helper.ViewData.ModelState[key].Errors)
				{
					retVal.Append(helper.Encode(err.ErrorMessage));
					count++;
					if (count == 1 && onlyFirst)
						break;
				}
				if (count == 1 && onlyFirst)
					break;
			}
			retVal.Append("</div>");
			return retVal.ToString();
		}

		public static MvcHtmlString FormControlGroup(this HtmlHelper helper, string idControl, string labelText, object controlContent)
		{
			StringBuilder result = new StringBuilder();
			result.AppendFormat("<div class='control-group'><label class='control-label'>{1} :</label><div class='controls'>{2}</div></div>", idControl, labelText, controlContent);

			return  MvcHtmlString.Create(result.ToString());
		}

		public static MvcHtmlString ClientIdFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
		{
			return MvcHtmlString.Create(htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression)));
		}

		public static MvcHtmlString FormatTextAreaFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
		{
			string result = htmlHelper.TextAreaFor(expression, new { placeholder = "Bloc infos", @class = "input-block-level" }).ToString();
			
			
			string fieldId = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression));
			result = string.Concat(result, string.Format("<span id=\"cog_{0}\" class=\"cog\" title=\"Selectionnez une partie du texte, puis saisissez un format de saisie ('c4', 'v2', '#25', etc. sans guillemets).\"></span>", fieldId));
			result = string.Concat(result, string.Format("<script type='text/javascript'>$(function () {{$('#cog_{0}').click(function (e) {{e.preventDefault();selectFormat($(\"#{0}\"));}});}});</script>", fieldId));

			return new MvcHtmlString(result);
		}

		public static MvcForm BeginFileForm(this HtmlHelper html)
		{
			return html.BeginForm(null, null, FormMethod.Post, new { enctype = "multipart/form-data" });
		}



		public static string GetFullPropertyName<T, TProperty>(Expression<Func<T, TProperty>> exp)
		{
			MemberExpression memberExp;

			if (!TryFindMemberExpression(exp.Body, out memberExp))
				return string.Empty;

			var memberNames = new Stack<string>();

			do
			{
				memberNames.Push(memberExp.Member.Name);
			}
			while (TryFindMemberExpression(memberExp.Expression, out memberExp));

			return string.Join(".", memberNames.ToArray());
		}

		public static bool TryFindMemberExpression(Expression exp, out MemberExpression memberExp)
		{
			memberExp = exp as MemberExpression;

			if (memberExp != null)
				return true;

			if (IsConversion(exp) && exp is UnaryExpression)
			{
				memberExp = ((UnaryExpression)exp).Operand as MemberExpression;

				if (memberExp != null)
					return true;
			}

			return false;
		}

		public static bool IsConversion(Expression exp)
		{
			return (exp.NodeType == ExpressionType.Convert || exp.NodeType == ExpressionType.ConvertChecked);
		}
	}
}