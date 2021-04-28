using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Fluke900Link.Containers;
using ScintillaNET;

namespace Fluke900Link.Lexers
{
    public class Fluke900Lexer
    {

        public const int StyleDefault = 0;
        public const int StyleKeyword = 1;
        //public const int StyleIdentifier = 1;
        public const int StyleNumber = 2;
        public const int StyleString = 3;
        public const int StyleComment = 4;
        public const int StyleInvalid = 5;

        private const int STATE_UNKNOWN = 0;
        private const int STATE_KEYWORD = 1;
        private const int STATE_NUMBER = 2;
        private const int STATE_STRING = 3;
        private const int STATE_COMMENT = 4;

        private HashSet<string> _keywords;
        private HashSet<string> _invalidKeywords;

        public void Style(Scintilla scintilla, int startPos, int endPos)
        {
            // Back up to the line start
            var line = scintilla.LineFromPosition(startPos);
            startPos = scintilla.Lines[line].Position;

            var length = 0;
            var state = STATE_UNKNOWN;

            // Start styling
            scintilla.StartStyling(startPos);
            while (startPos < endPos)
            {
                var c = Char.ToLower((char)scintilla.GetCharAt(startPos));
                string s = scintilla.GetTextRange(startPos, 50);
                //var c = (char)scintilla.GetCharAt(startPos);

            REPROCESS:
                switch (state)
                {
                    case STATE_UNKNOWN:
                        if (c == ';')
                        {
                            state = STATE_COMMENT;
                            goto REPROCESS;
                        }
                        else if (c == '\r' || c == '\n')
                        {
                            state = STATE_UNKNOWN;
                            scintilla.SetStyling(1, StyleDefault);
                        }
                        else if (c == '"')
                        {
                            // Start of "string"
                            scintilla.SetStyling(1, StyleString);
                            state = STATE_STRING;
                        }
                        else if (Char.IsDigit(c))
                        {
                            state = STATE_NUMBER;
                            goto REPROCESS;
                        }
                        else if (Char.IsLetter(c) || c == '_')
                        {
                            state = STATE_KEYWORD;
                            goto REPROCESS;
                        }
                        else
                        {
                            // Everything else
                            scintilla.SetStyling(1, StyleDefault);
                        }
                        break;

                    case STATE_STRING:
                        if (c == '"')
                        {
                            length++;
                            scintilla.SetStyling(length, StyleString);
                            length = 0;
                            state = STATE_UNKNOWN;
                        }
                        else
                        {
                            length++;
                        }
                        break;

                    case STATE_NUMBER:
                        if (Char.IsDigit(c) || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F') || c == 'x')
                        {
                            length++;
                        }
                        else
                        {
                            scintilla.SetStyling(length, StyleNumber);
                            length = 0;
                            state = STATE_UNKNOWN;
                            goto REPROCESS;
                        }
                        break;

                    case STATE_COMMENT:
                        if (c == '\r' || c == '\n')
                        {
                            state = STATE_UNKNOWN;
                            //scintilla.SetStyling(1, StyleDefault);
                            goto REPROCESS;
                        }
                        else
                        {
                            scintilla.SetStyling(1, StyleComment);
                        }
                        break;

                    case STATE_KEYWORD:
                        if (Char.IsLetterOrDigit(c) || c == '_')
                        {
                            length++;
                        }
                        else
                        {
                            var style = StyleDefault;
                            var identifier = scintilla.GetTextRange(startPos - length, length).ToLower();
                            if (_invalidKeywords.Contains(identifier))
                            {
                                style = StyleInvalid;
                            }
                            else if (_keywords.Contains(identifier))
                            {
                                style = StyleKeyword;
                            }

                            scintilla.SetStyling(length, style);
                            length = 0;
                            state = STATE_UNKNOWN;
                            goto REPROCESS;
                        }
                        break;
                }

                startPos++;
            }
        }

        public Fluke900Lexer(KnownFileType fileType)
        {
            //string keys = "";
            //string badkeys = "";


            //we will derive our good and bad keywords directly from our compatibilty class
            //IEnumerable<string> s = LibraryFileCommands.GetValidCommands(fileType).Select(c => Enum.GetName(typeof(FileCommand), c).ToLower());
            this._keywords = new HashSet<string>(LibraryFileCommands.GetValidCommands(fileType).Select(c => Enum.GetName(typeof(FileCommand), c).ToLower()));
            this._invalidKeywords = new HashSet<string>(LibraryFileCommands.GetInvalidCommands(fileType).Select(c => Enum.GetName(typeof(FileCommand), c).ToLower()));
            
            
            
            //switch (fileType)
            //{
            //    case KnownFileType.Lib:
            //        keys = "activity clip_chk c_sum gate gate_delay ignore load name rd_drv rdsim rdt_enable rdtest shadow size s_time sync_cond sync_game sync_gr_end sync_ignore sync_pat sync_pins sync_reset_off sync_rnd sync_vect thrsld";
            //        badkeys = "comment compare display end end_function f_mask function if then jump loc_file reset sound test trigger t_time";
            //        break;
            //    case KnownFileType.Loc:
            //        keys = "activity clip_chk compare c_sum display f_mask gate gate_delay ignore load name rd_drv rdsim rdt_enable reset shadow size s_time thrsld trigger t_time";
            //        badkeys = "comment end end_function function if then jump loc_file  rdtest sound sync_cond sync_gate sync_gr_end sync_ignore sync_pat sync_pins sync_reset_off sync_vect test";
            //        break;
            //    case KnownFileType.Seq:
            //        keys = "comment display end end_function function gate gate_delay if then jump loc_file rdsim size sound test";
            //        badkeys = "activity clip_chk c_sum f_mask ignore load name rd_drv rdt_enable rdtest reset shadow s_time sync_cond sync_gate sync_gr_end sync_ignore sync_pat sync_pins sync_reset_off sync_rnd sync_vect thrsld trigger t_time";
            //        break;
            //}
            // Put keywords in a HashSet
            //var list = Regex.Split(keys ?? string.Empty, @"\s+").Where(l => !string.IsNullOrEmpty(l));
            //this.keywords = new HashSet<string>(keys.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries));
            //this.invalidKeywords = new HashSet<string>(badkeys.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
        }
    }
}
