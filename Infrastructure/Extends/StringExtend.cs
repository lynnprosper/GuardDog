﻿using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Infrastructure
{
    /// <summary>
    /// String类扩展   
    /// </summary>
    public static partial class StringExtend
    {
        /// <summary>
        /// 如果为null则返回String.Empty
        /// </summary>
        /// <param name="source">源</param>
        /// <returns></returns>
        public static string NullToEmpty(this string source)
        {
            return source.NullThenEmpty();
        }

        /// <summary>
        /// 如果为null则返回String.Empty
        /// </summary>
        /// <param name="source">源</param>
        /// <returns></returns>
        public static string NullThenEmpty(this string source)
        {
            if (source == null)
            {
                return string.Empty;
            }
            return source;
        }

        /// <summary>
        /// 如果为null，则返回value的值
        /// </summary>
        /// <param name="source">源</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static string NullThen(this string source, string value)
        {
            if (source.IsNullOrEmpty())
            {
                return value;
            }
            return source;
        }

        /// <summary>
        /// 判断字符串是否为null或Empty
        /// </summary>
        /// <param name="source">源</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string source)
        {
            return string.IsNullOrEmpty(source);
        }

        /// <summary>
        /// 当值与when条件相同则返回then值
        /// </summary>
        /// <param name="source"></param>
        /// <param name="when"></param>
        /// <param name="then"></param>
        /// <returns></returns>
        public static string WhenThen(this string source, string when, string then)
        {
            if (source == when)
            {
                return then;
            }
            return source;
        }

        /// <summary>
        /// 转换为小写
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToLowerIfNoNull(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return source;
            }
            return source.ToLower();
        }

        /// <summary>
        /// 转换为大写
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToUpperIfNoNull(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return source;
            }
            return source.ToUpper();
        }

        /// <summary>
        /// 转换为Bool 类型
        /// 失败返回默认值 false
        /// </summary>
        /// <param name="source">源</param>
        /// <returns></returns>
        public static bool ToBool(this string source)
        {
            bool value = false;
            bool.TryParse(source, out value);
            return value;
        }

        /// <summary>
        /// 转换为Bool 类型
        /// 失败则直接返回默认值default
        /// </summary>
        /// <param name="source">源</param>
        /// <param name="defalut">默认值</param>
        /// <returns></returns>
        public static bool ToBool(this string source, bool defalut)
        {
            bool value = false;
            if (bool.TryParse(source, out value))
            {
                return value;
            }
            return defalut;
        }

        /// <summary>
        /// 转换为整数
        /// 失败返回默认值0
        /// </summary>
        /// <param name="source">源</param>
        /// <returns></returns>
        public static int ToInt32(this string source)
        {
            int value = 0;
            int.TryParse(source, out value);
            return value;
        }

        /// <summary>
        /// 转换为整数
        /// 失败则直接返回默认值default
        /// </summary>
        /// <param name="source">源</param>
        /// <param name="defalut">默认值</param>
        /// <returns></returns>
        public static int ToInt32(this string source, int defalut)
        {
            int value = 0;
            if (int.TryParse(source, out value))
            {
                return value;
            }
            return defalut;
        }

        /// <summary>
        /// 是否匹配正则表达式参数
        /// </summary>
        /// <param name="source">源</param>
        /// <param name="pattert">regex参数</param>
        /// <returns></returns>
        public static bool IsMatch(this string source, string pattert)
        {
            source = source.NullThenEmpty();
            pattert = pattert.NullThenEmpty();
            return System.Text.RegularExpressions.Regex.IsMatch(source, pattert);
        }

        /// <summary>
        /// 匹配正则表示式
        /// </summary>
        /// <param name="source">源</param>
        /// <param name="pattert">regex参数</param>
        /// <returns></returns>
        public static string Match(this string source, string pattert)
        {
            source = source.NullThenEmpty();
            pattert = pattert.NullThenEmpty();
            return System.Text.RegularExpressions.Regex.Match(source, pattert).Value;
        }

        /// <summary>
        /// 匹配正则表示式
        /// </summary>
        /// <param name="source">源</param>
        /// <param name="pattert">regex参数</param>
        /// <returns></returns>
        public static string[] Matches(this string source, string pattert)
        {
            return Regex
                .Matches(source.NullThenEmpty(), pattert.NullThenEmpty())
                .Cast<Match>()
                .Select(item => item.Value)
                .ToArray();
        }

        /// <summary>
        /// 重复填充字符
        /// </summary>
        /// <param name="source">源</param>
        /// <param name="totalLength">填充后的长度</param>
        /// <returns></returns>
        public static string Pad(this char source, int totalLength)
        {
            return string.Empty.PadRight(totalLength, source);
        }

        /// <summary>
        /// 以GB2312编码获取字符串的长度
        /// 一个中文占两个长度 英文一个长度
        /// </summary>
        /// <param name="source">源</param>
        /// <returns></returns>
        public static int LengthGB2312(this string source)
        {
            return Encoding.GetEncoding("gb2312").GetByteCount(source.NullThenEmpty());
        }

        /// <summary>
        /// 以GB2312编码切割字符串
        /// </summary>
        /// <param name="source">源</param>
        /// <param name="lengthGB2312">保留的长度</param>
        /// <returns></returns>
        public static string SubstringGB2312(this string source, int lengthGB2312)
        {
            var encoding = Encoding.GetEncoding("gb2312");
            source = source.NullThenEmpty();

            if (lengthGB2312 >= source.LengthGB2312())
            {
                return source;
            }

            int index = 0;
            int length = 0;
            var chars = source.ToCharArray();

            while (length < lengthGB2312)
            {
                length += encoding.GetByteCount(chars, index, 1);
                if (length == lengthGB2312)
                {
                    return new string(chars, 0, index + 1);
                }
                else if (length > lengthGB2312)
                {
                    return new string(chars, 0, index);
                }
                index++;
            }
            return string.Empty;
        }

        /// <summary>
        /// 以GB2312编码切割字符串
        /// </summary>
        /// <param name="source">源</param>
        /// <param name="lengthGB2312">保留的长度</param>
        /// <param name="padChar">填充字符</param>
        /// <returns></returns>
        public static string SubstringGB2312(this string source, int lengthGB2312, char padChar)
        {
            source = source.NullThenEmpty().SubstringGB2312(lengthGB2312);
            int count = lengthGB2312 - source.LengthGB2312();
            if (count == 0)
            {
                return source;
            }
            return string.Concat(source, padChar.Pad(count));
        }

        /// <summary>
        /// 字符串马赛克
        /// 左边长度大于字符串总长度，整个字符串打码
        /// 右边长度大于左边长度+字符串长度时，除左边长度外其它字符都打码
        /// </summary>
        /// <param name="source"></param>
        /// <param name="mask">马赛克</param>
        /// <param name="left">左边长度</param>
        /// <param name="length">右边保留长度</param>
        /// <returns></returns>
        public static string MaskAsCenter(this string source, char mask, int left, int right)
        {
            if (source.IsNullOrEmpty())
            {
                return source;
            }
            if (source.Length < left)
            {
                left = 0;
            }
            var length = source.Length - left - right;
            length = length > 0 ? length : (source.Length - left);
            return source.MaskAs(mask, left, length);
        }

        /// <summary>
        /// 字符串马赛克
        /// </summary>
        /// <param name="source"></param>
        /// <param name="mask">马赛克</param>
        /// <param name="left">左边长度</param>
        /// <param name="length">打码长度</param>
        /// <returns></returns>
        public static string MaskAs(this string source, char mask, int left, int length)
        {
            if (source.IsNullOrEmpty())
            {
                return source;
            }
            var pattern = string.Format(@"(?<=^.{{{0}}}).{{{1}}}", left, length);
            return Regex.Replace(source, pattern, mask.Pad(length));
        }

        /// <summary>
        /// 省略
        /// </summary>
        /// <param name="source"></param>
        /// <param name="lengthGB2312"></param>
        /// <returns></returns>
        public static string EllipsisGB2312(this string source, int lengthGB2312)
        {
            if (source.LengthGB2312() > lengthGB2312)
            {
                return source.SubstringGB2312(lengthGB2312) + " ..";
            }
            return source;
        }

        /// <summary>
        /// 获取和目标字符串的相似度
        /// </summary>
        /// <param name="source">源</param>
        /// <param name="target">目标字符串</param>
        /// <returns></returns>
        public static decimal GetSimilarityWith(this string source, string target)
        {
            var Kq = 2m;
            var Kr = 1m;
            var Ks = 1m;

            var sourceArray = source.ToCharArray();
            var destArray = target.ToCharArray();

            //获取交集数量
            var q = sourceArray.Intersect(destArray).Count();
            var s = sourceArray.Length - q;
            var r = destArray.Length - q;
            return Kq * q / (Kq * q + Kr * r + Ks * s);
        }

        /// <summary>
        /// HMACSHA1 加密
        /// </summary>
        /// <param name="source">待加密字符串</param>
        /// <param name="Key">秘钥</param>
        /// <returns></returns>
        public static string ToBase64hmac(this string source, string Key)
        {
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(Key))
            {
                return string.Empty;
            }
            HMACSHA1 myHMACSHA1 = new HMACSHA1(Encoding.UTF8.GetBytes(Key));
            byte[] byteText = myHMACSHA1.ComputeHash(Encoding.UTF8.GetBytes(source));
            return System.Convert.ToBase64String(byteText);
        }
    }
}