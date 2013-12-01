﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Resources;
using System.Web.Mvc;
using HtmlTags;
using HtmlTags.Extended.Attributes;

namespace MvcPowerTools.HtmlConventions
{
    /// <summary>
    /// Should be added at the end
    /// </summary>
    public class DataAnnotationConventions:CommonHtmlConventions
    {
        public DataAnnotationConventions()
        {
            Order = int.MaxValue;
        }
        public override void Configure(HtmlConventionsManager conventions)
        {
            base.Configure(conventions);
            conventions.Labels
                .ForModelWithAttribute<DisplayAttribute>()
                .Modify((tag, info) =>
                {
                    var attr = info.GetAttribute<DisplayAttribute>();
                    return tag.Text(attr.Name);                   
                    
                });

            conventions.Editors
                .ForModelWithAttribute<DataTypeAttribute>()
                    .Modify((tag, info) =>
                    {
                        var attr = info.GetAttribute<DataTypeAttribute>();
                        var input = tag.FirstInputTag();
                        if (attr.DataType == DataType.Text || attr.DataType == DataType.MultilineText)
                        {
                            input.MultilineMode();
                            return tag;
                        }

                        if (attr.DataType == DataType.Password)
                        {
                            input.PasswordMode();
                            return tag;
                        }
                        if (attr.DataType == DataType.EmailAddress)
                        {
                            input.EmailMode();
                            return tag;
                        }

                        if (attr.DataType == DataType.Upload)
                        {
                            input.FileUploadMode();
                            return tag;
                        }
                        return tag;
                    });

            conventions.Editors
                .ForModelWithAttribute<HiddenInputAttribute>()
                .Build(info =>
                {
                    return new HiddenTag()
                        .Name(info.HtmlName)
                        .IdFromName()
                        .Value(info.ValueAsString);
                })
                ;

            conventions.Editors

                .ForModelWithAttribute<StringLengthAttribute>()
                    .Modify((tag, info) =>
                    {
                        var attr = info.GetAttribute<StringLengthAttribute>();
                        var input=tag.FirstInputTag();
                        input.Attr("maxlength", attr.MaximumLength);
                        if (attr.MinimumLength > 0)
                            input.Attr("minlength", attr.MinimumLength);
                        return tag;
                    })
                
                .If(m =>
                {
                    int tc;
                    if (!m.Type.IsNullable())
                    {
                        tc = (int) Type.GetTypeCode(m.Type);
                    }
                    else
                    {
                        tc = (int) Type.GetTypeCode(Nullable.GetUnderlyingType(m.Type));
                    }

                    //number type range
                    return (tc >= 5 && tc <= 15);
                } )
                    .Modify((tag, info) =>
                    {
                        tag.FirstInputTag().NumberMode();
                        return tag;
                    })
                
                ;
                   
        }
    }
}