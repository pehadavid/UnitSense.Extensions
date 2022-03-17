using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace UnitSense.Extensions.Extensions
{
    public static class StringExt
    {
        /// <summary>
        /// Tronque une chaine. Cette méthode préserve les mots. 
        /// </summary>
        /// <param name="source">Chaine à tronquer</param>
        /// <param name="length">Longueur</param>
        /// <param name="endString">Chaine à rajouter après opération</param>
        /// <returns>Une chaine tronquée selon les params</returns>
        public static string Truncate(this string source, int length, string endString = "...")
        {
            string text = source.StripTags();
            if (text.Length <= (length + endString.Length))
                return text;

            text = Regex.Replace(text, "\n+", " ");

            try
            {
                text = new string(text.Substring(0, length).Reverse().ToArray());
                text = new string(text.Substring(Math.Abs(text.IndexOf(" ")), (text.Length - text.IndexOf(" "))).Reverse().ToArray());
                text = text.Trim() + endString;
            }
            catch (Exception)
            {

                return source;
            }



            return text;
        }

        /// <summary>
        /// Tronque une chaine. Cette méthode ne préserve pas les mots et donne une chaine de longueur exacte
        /// </summary>
        /// <param name="source">Chaine à tronquer</param>
        /// <param name="length">Longueur exactement retournée </param>
        /// <returns></returns>
        public static string HardTruncate(this string source, int length, string endString = "")
        {
            string text = source.StripTags();
            if (text.Length <= length)
                return text;

            text = Regex.Replace(text, "\n+", " ");
            text = text.Trim();

            var returnValue = text.Substring(0, length) + endString;

            return returnValue;
        }

        /// <summary>
        /// Remplace la première occurence d'un terme dans une chaine par une autre chaine
        /// </summary>
        /// <param name="haystack">Chaine a inspecter</param>
        /// <param name="needle">Sous-chaine à remplcaer</param>
        /// <param name="replacement">Sous-chaine de remplacement</param>
        /// <returns>Chaine traitée</returns>
        public static string ReplaceFirst(this string haystack, string needle, string replacement)
        {
            int pos = haystack.IndexOf(needle, System.StringComparison.Ordinal);
            if (pos < 0) return haystack;
            return haystack.Substring(0, pos) + replacement + haystack.Substring(pos + needle.Length);
        }

        public static string ReplaceLast(this string haystack, string needle, string replacement)
        {
            string reverse_haystack = new string(haystack.Reverse().ToArray());
            string reverse_needle = new string(needle.Reverse().ToArray());
            string reverse_replacement = string.IsNullOrWhiteSpace(replacement) ? replacement : new string(replacement.Reverse().ToArray());

            string reverse_result = reverse_haystack.ReplaceFirst(reverse_needle, reverse_replacement);

            return new string(reverse_result.Reverse().ToArray());
        }
        /// <summary>
        /// Remplace la totalité des occurences d'un terme dans chaine donnée
        /// </summary>
        /// <param name="haystack">Chaine à inspecter</param>
        /// <param name="needle">Sous-chaine a remplacer</param>
        /// <param name="replacement">Sous-chaine de remplacement</param>
        /// <returns>Chaine traitée</returns>
        public static string ReplaceAll(this string haystack, string needle, string replacement)
        {
            int pos;
            // Avoid a possible infinite loop
            if (needle == replacement) return haystack;
            while ((pos = haystack.IndexOf(needle)) > 0)
                haystack = haystack.Substring(0, pos) + replacement + haystack.Substring(pos + needle.Length);
            return haystack;
        }
        /// <summary>
        /// PErmet de supprimer les doubles espaces dans un text
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string TrimDoubleSpace(this string str)
        {
            Regex regex = new Regex(@"(\s){2,}");
            str = regex.Replace(str, " ");
            return str;
        }
        /// <summary>
        /// Permet de nettoyer les balises HTML d'une chaine. 
        /// </summary>
        /// <param name="Input">Chaine à nettoyer</param>
        /// <param name="AllowedTags">Tableau contenant les balises autorisées,  qui ne seront pas nettoyées</param>
        /// <returns>Chaine traitée</returns>
        public static string StripTags(this string Input, string[] AllowedTags = null)
        {
            if (string.IsNullOrWhiteSpace(Input))
                return string.Empty;

            Regex StripHTMLExp = new Regex(@"(<\/?[^>]+>)");
            string Output = Input;

            MatchCollection matchCollection = StripHTMLExp.Matches(Input);
            foreach (Match Tag in matchCollection)
            {
                string HTMLTag = Tag.Value.ToLower();
                bool IsAllowed = false;

                if (AllowedTags != null)
                {
                    foreach (string AllowedTag in AllowedTags)
                    {
                        int offset = -1;

                        // Determine if it is an allowed tag
                        // "<tag>" , "<tag " and "</tag"
                        if (offset != 0) offset = HTMLTag.IndexOf('<' + AllowedTag + '>');
                        if (offset != 0) offset = HTMLTag.IndexOf('<' + AllowedTag + ' ');
                        if (offset != 0) offset = HTMLTag.IndexOf("</" + AllowedTag);

                        // If it matched any of the above the tag is allowed
                        if (offset == 0)
                        {
                            IsAllowed = true;
                            break;
                        }
                    }
                }

                // Remove tags that are not allowed
                if (!IsAllowed)
                    Output = ReplaceFirst(Output, Tag.Value, "");
            }

            return Output;
        }

        public static string RemoveTags(this string input, string[] tagsToRemove)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            Regex stripHTMLExp = new Regex(@"(<\/?[^>]+>)");
            string output = input;

            MatchCollection matchCollection = stripHTMLExp.Matches(input);
            foreach (Match tag in matchCollection)
            {
                foreach (var tagToRemove in tagsToRemove)
                {
                    string htmlTag = tag.Value.ToLower();
                    int offset = -1 * htmlTag.IndexOf('<' + tagToRemove + '>') * htmlTag.IndexOf('<' + tagToRemove + ' ') * htmlTag.IndexOf("</" + tagToRemove);
                    bool mustBeRemoved = offset == 0;


                    if (mustBeRemoved)
                    { output = ReplaceFirst(output, tag.Value, ""); break; }



                }


            }

            return output;
        }

        /// <summary>
        /// DEPRECATED
        /// </summary>
        /// <param name="Input"></param>
        /// <param name="AllowedTags"></param>
        /// <returns></returns>
        public static string StripTagsAndAttributes(this string Input, string[] AllowedTags)
        {
            /* Remove all unwanted tags first */
            string Output = StripTags(Input, AllowedTags);

            /* Lambda functions */
            MatchEvaluator HrefMatch = m => m.Groups[1].Value + "href..;,;.." + m.Groups[2].Value;
            MatchEvaluator ClassMatch = m => m.Groups[1].Value + "class..;,;.." + m.Groups[2].Value;
            MatchEvaluator UnsafeMatch = m => m.Groups[1].Value + m.Groups[4].Value;

            /* Allow the "href" attribute */
            Output = new Regex("(<a.*)href=(.*>)").Replace(Output, HrefMatch);

            /* Allow the "class" attribute */
            Output = new Regex("(<a.*)class=(.*>)").Replace(Output, ClassMatch);

            /* Remove unsafe attributes in any of the remaining tags */
            Output = new Regex(@"(<.*) .*=(\'|\""|\w)[\w|.|(|)]*(\'|\""|\w)(.*>)").Replace(Output, UnsafeMatch);

            /* Return the allowed tags to their proper form */
            Output = ReplaceAll(Output, "..;,;..", "=");

            return Output;
        }
        /// <summary>
        /// Permet de remplacer plusieurs éléments au sein d'une meme chaine
        /// </summary>
        /// <param name="source">chaine de départ</param>
        /// <param name="Target">Tableau d'éléments recherché dans la chaine, à remplacer</param>
        /// <param name="Replacement">Tableau des éléments qui doivent remplacer les éléments ciblés</param>
        /// <returns></returns>
        public static string Replace(this string source, string[] Target, string[] Replacement)
        {
            for (int i = 0; i < Target.Length; i++)
            {
                string o = Target[i];
                string t = Replacement[i];
                source = source.Replace(o, t);
            }

            return source;
        }


        /// <summary>
        /// Ajoute un sous-chaine en fin de chaine
        /// </summary>
        /// <param name="source"></param>
        /// <param name="StringToAppend">Sous chaine a ajouter</param>
        /// <returns></returns>
        public static string Append(this string source, string StringToAppend)
        {
            return source + StringToAppend;
        }


        /// <summary>
        /// Permet d'agreger une chaine a l'aide d'un délimiteur
        /// </summary>
        /// <param name="source"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static string Aggregate(this IEnumerable<string> source, string delimiter)
        {
            return source.Any() ? source.Aggregate((x, z) => x + delimiter + z) : string.Empty;
        }


        /// <summary>
        /// Permet de generer une chaine aléatoire, a partir d'une chaine source
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Randomize(this string source)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            int size = 16;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        /// <summary>
        /// Permet de generer une chaine aléatoirement
        /// </summary>
        /// <param name="size">Taille attendue de la chaine</param>
        /// <returns></returns>
        public static string Randomize(int size)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random(Environment.TickCount);
            char ch;

            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        /// <summary>
        /// transforme une chaine de caractère en url SEO
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToUrlFormat(this string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return string.Empty;
            return Regex.Replace(source, @"[^\w\d]", "-").ToLowerInvariant().RemoveDiacritics();
        }

        /// <summary>
        /// Permet de placer le premier char d'une chaine en majuscule
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string UcFirst(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return string.Empty;
            }
            char[] a = source.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        /// <summary>
        /// Transforme les char. accentués d'une chaine en leurs équivalent non accentués.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static String RemoveDiacritics(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return s;
            String normalizedString = s.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in normalizedString)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    stringBuilder.Append(c);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Permet de mettre au pluriel une chaine en fonction d'un nombre indiqué
        /// </summary>
        /// <param name="singleString">la chaine au singulier</param>
        /// <param name="nb">Nombre</param>
        /// <param name="pluralChar">le char définissant le pluriel</param>
        /// <returns>La chaine d'origine au pluriel si nb > 1</returns>
        public static string Pluralize(this string singleString, int? nb, char pluralChar)
        {
            if (nb != null && nb > 1)
                return singleString + pluralChar;

            return singleString;
        }

        /// <summary>
        /// Transforme tous les retours chariots en sauts de ligne HTML
        /// </summary>
        /// <param name="me">Chaine à traiter</param>
        /// <returns></returns>
        public static string NewLineToBr(this string me)
        {
            return me.Replace(Environment.NewLine, "<br />").Replace("\n", "<br />");
        }

        /// <summary>
        /// Transforme tous les sauts de ligne HTML en retour chariots
        /// </summary>
        /// <param name="me"></param>
        /// <returns></returns>
        public static string BrToNewLine(this string me)
        {
            return me.Replace("<br />", Environment.NewLine);
        }

        /// <summary>
        /// Multiplie une chaine de caractère
        /// </summary>
        /// <param name="me"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string Multiply(this string me, int count)
        {
            string mulValue = string.Empty;
            for (int i = 0; i < count; i++)
            {
                mulValue += me;
            }
            return mulValue;
        }

        /// <summary>
        /// Indique si la chaine est nulle, vide, ou composée uniquement d'espaces
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNullOrWhitespace(this string source)
        {
            return string.IsNullOrWhiteSpace(source);
        }

        /// <summary>
        /// Compte le nombre de mots de la chaine donnée
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int CountWords(this string source)
        {
            MatchCollection collection = Regex.Matches(source, @"[\S]+");
            return collection.Count;
        }


        public static bool IsNumeric(this string source)
        {
            int parsed;
            return int.TryParse(source, out parsed);
        }


        /// <summary>
        /// Supprime les caractères de retours à la ligne et d'échappement
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ClearEscapeSequence(this string str)
        {
            return Regex.Replace(str, "(\\r\\n|\\r|\\n)", string.Empty);
        }

        /// <summary>
        /// Remplace les entités html d'une chaine (ex: &nbsp;)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ClearHtmlEntities(this string str)
        {
            return Regex.Replace(str, "(&[a-zA-Z]+;)", "x");
        }

        /// <summary>
        /// Supprime l'empilement de caractères unicodes (zalgo) 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ZalgoKiller(this string str)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var c in str.Normalize(NormalizationForm.FormC))
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.EnclosingMark &&
                    CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }

            return sb.ToString();
        }


        /// <summary>
        /// Supprime tous les espaces d'un string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string RemoveWhitespace(this string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !char.IsWhiteSpace(c))
                .ToArray());
        }

        public static string GetValueOrDefault(this string input, string defaultValue)
        {
            return input.IsNullOrWhitespace() ? defaultValue : input;
        }
        public static bool Contains(this string str, StringOperations operation, params string[] terms)
        {
            switch (operation)
            {
                case StringOperations.AND:
                    var b = true;
                    foreach (string term in terms)
                    {
                        b &= str.Contains(term);
                    }
                    return b;
                    break;
                case StringOperations.XOR:
                    var bxor = false;
                    foreach (string term in terms)
                    {
                        bxor ^= str.Contains(term);
                    }
                    return bxor;
                    break;
                case StringOperations.OR:
                    var bor = false;
                    foreach (string term in terms)
                    {
                        bor |= str.Contains(term);
                    }
                    return bor;


            }

            return false;
        }
        public enum StringOperations
        {

            OR = 0x0,
            AND = 0x1,
            XOR = 0x2



        }
    }
}
