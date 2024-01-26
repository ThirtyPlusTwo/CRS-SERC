private readonly string TEAM_TAG = "XXX";                               //Your Team Tag (3 chracters), if you are not in a team yet, keep this as it is.
private readonly string DRIVER_NAME = "Guest";                          //Your name
private readonly int DRIVER_NUMBER = 99;                                //Your number (0-99)
private const float DEFAULT_SUSPENSION_STRENGTH_F = 20f;                //Setup your default front suspensions strength
private const float DEFAULT_SUSPENSION_STRENGTH_R = 20f;                //Setup your default rear suspensions strength
private const string DISPLAY_NAME = "Driver LCD";
private const string BRAKELIGHT_GROUP_NAME = "Brakelight";
private const string DRS_LIGHTS_GROUP_NAME = "DRS Lights";
private const string ERS_LIGHTS_GROUP_NAME = "ERS Lights";
private const string DRAFTING_SENSOR_NAME = "Drafting Sensor";
private const string MIRROR_SENSOR_RIGHT_NAME = "Mirror Sensor Right";
private const string MIRROR_SENSOR_LEFT_NAME = "Mirror Sensor Left";
private readonly int? COCKPIT_DISPLAY_INDEX = null;                     //If you wanna use a cockpit display to show dashboard info (0, 1, 2, 3 or null)
private readonly Color DEFAULT_FONT_COLOR = new Color(255, 255, 255);   //Font Color (R, G, B)
private const string TEXT_DISPLAY_NAME = "Text LCD";                    //Optional Text-Based LCD, for HudLcd Plugin
private const string RANK_DISPLAY_NAME = "Rank LCD";                    //Optional Text-Based LCD, for HudLcd Plugin
private const string TEXT_DISPLAY_HUDLCD = "hudlcd:-0.7:-0.35:0.9:White:1";
private const string RANK_DISPLAY_HUDLCD = "hudlcd:0.45:0.9:1:White:1";

string ţ="12.5.0";const int Ţ=3000;const int š=1000;const int Ť=1000;const float ť=80f;const float ż=95f;char Ž='\u25BC'
;char Ż='\u25B2';char ź='\u25BA';char Ź='\u25C4';const char Ÿ='\u2588';const char ŷ='\u2592';const char Ŷ='\u2591';const
float ŵ=1.7f;bool Ŵ;IMyMotorSuspension[]ų;IMyShipController Ų;List<IMyTextSurface>ű;IMyTextSurface Ű;IMyTextSurface ů;
IMyTextSurface Ů;IMyRadioAntenna ŭ;IMySensorBlock Ŭ;IMySensorBlock ū;IMySensorBlock Ū;List<IMyGyro>ũ;bool Ũ;bool ŧ;bool Ŧ;
StringBuilder ş;ď ŉ;List<IMyLightingBlock>ŝ;List<IMyLightingBlock>Ň;List<IMyLightingBlock>ņ;List<IMyLightingBlock>Ņ;Ħ ń;long Ń=-1;
IMyBroadcastListener ł;int Ł;int ŀ;DateTime Ŀ;float ň;float ľ=1f;bool Ľ=false;int ļ;bool Ļ;List<MyDetectedEntityInfo>ĺ;List<
MyDetectedEntityInfo>Ĺ;X ĸ;Program(){ŉ=new ď();ĸ=new X(new char[]{'-','\\','|','/'},150);try{Ɓ();ƌ();ƕ();Ɣ();Ǝ();É();Ê();Í();ķ();º();ß();Ã()
;Á();}catch(Exception ex){Ŵ=true;Echo(ex.Message);return;}Runtime.UpdateFrequency=UpdateFrequency.Update1;Ŀ=DateTime.Now;
}void ķ(){ĺ=new List<MyDetectedEntityInfo>();ū=(IMySensorBlock)GridTerminalSystem.GetBlockWithName(
MIRROR_SENSOR_RIGHT_NAME);if(ū!=null){ū.DetectSmallShips=true;ū.DetectLargeShips=false;ū.DetectPlayers=false;ū.LeftExtend=12.5f;ū.RightExtend=
0.5f;ū.FrontExtend=50;ū.BackExtend=5;ū.TopExtend=5;ū.BottomExtend=5;}Ū=(IMySensorBlock)GridTerminalSystem.GetBlockWithName(
MIRROR_SENSOR_LEFT_NAME);if(Ū!=null){Ū.DetectSmallShips=true;Ū.DetectLargeShips=false;Ū.DetectPlayers=false;Ū.LeftExtend=0.5f;Ū.RightExtend=
12.5f;Ū.FrontExtend=50;Ū.BackExtend=5;Ū.TopExtend=5;Ū.BottomExtend=5;}}void Save(){}void Main(string Ü,UpdateType Ŋ){if(Ŵ){
return;}var Ş=DateTime.Now;ň=(float)(Ş-Ŀ).TotalMilliseconds/1000;Echo($"Running FSESS {ţ}");Ý(Ü);Ʊ();Ƌ();ś();Ʋ();Ƅ();Ɔ();Ř();Ŕ
();ƅ();Ŝ();Ŀ=Ş;}void Ŝ(){if(!Ļ){return;}if(Vector3D.Dot(Ų.GetNaturalGravity(),Ų.WorldMatrix.Up)>0){for(int J=0;J<ũ.Count;
J++){ũ[J].GyroOverride=true;}return;}if(ũ[0].GyroOverride){for(int J=0;J<ũ.Count;J++){ũ[J].GyroOverride=false;}Ļ=false;}}
void ś(){switch(ŉ.Ķ){case ę.ė:Ś();break;case ę.Ė:ř();break;default:if(!Ũ){Ų.HandBrake=false;Ö(ż);}break;}}void Ś(){ŧ=false;Ŧ
=false;Ų.HandBrake=Ų.GetShipSpeed()>50;Ö(50f);}void ř(){ŧ=false;Ŧ=false;Ũ=false;Ų.HandBrake=true;}void Ř(){var ŗ=IGC.
UnicastListener;if(!ŗ.HasPendingMessage){Ł-=(int)(ň*1000);if(ł.HasPendingMessage&&Ł<=0){var Ŗ=ł.AcceptMessage();if(Ŗ.Tag=="Address"){Ń=
Convert.ToInt64(Ŗ.Data.ToString());IGC.SendUnicastMessage(Ń,"Register",$"{Me.CubeGrid.CustomName};{IGC.Me}");}}return;}while(ŗ.
HasPendingMessage){var ŕ=ŗ.AcceptMessage();if(ŕ.Tag=="RaceData"){ŉ.Ĭ(ŕ.Data.ToString());}if(ŕ.Tag=="Argument"){Ý(ŕ.Data.ToString());}}Ł=Ţ
;}void Ŕ(){ş.Clear();const int œ=21;const int Œ=8;const int ő=œ-6;var Ő=new char[œ,Œ];var æ=Ų.GetShipSpeed();var ŏ=ń.Ĥ;
var Ŏ=$"{Math.Floor(æ)}m/s";var R=Q('E',8,ľ,1);var ō=Q(ŏ,8,ń.ã,100);var Ō=ŉ.Ĵ==ù.Ì?"<CLEAR>":"<RAIN>";var Š=ŉ.ĳ<100?
$"RoR:{ŉ.ĳ,2}%":$"RoR:{ŉ.ĳ,3}";var ŋ=ŉ.Ĳ<=0?$"{Ō} {Š.PadLeft(ő-1-Ō.Length)}":$" <RAIN IN {ŉ.Ĳ,2}s> ";var ž=(ŧ?"DRS":"   ")+" "+(Ŧ?"ERS"
:"   ")+" "+(Ľ?"DFT":"   ")+" "+(Ũ?"PIT":"   ");var Ƭ=g(true);var ƪ=g(false);var Ʃ=ƪ.PadRight((int)Math.Ceiling((float)ő/
2)-(int)Math.Ceiling((float)Ŏ.Length/2))+Ŏ+Ƭ.PadLeft((int)Math.Floor((float)ő/2)-(int)Math.Floor((float)Ŏ.Length/2));ĸ.í(
ň);var ƨ=$"S1{a(ŉ.İ)}";var Ƨ=$"S2{a(ŉ.į)}";var Ʀ=$"S3{a(ŉ.Į)}";var ƥ=new string[]{$"   {Ʃ}   ",$"   {ž}   ",
$"   P:{ŉ.Ď:00}/{ŉ.č:00} L:{(ŉ.Č):00}/{ŉ.Ğ:00}   ",$"   TIME: {ŉ.Ġ}   ",$"   BEST: {ŉ.ĵ}   ",$"   PREV: {ŉ.ĭ}   ",$"                     ",$"   {ŋ}   ",(Ł<=0)?
$"    NO CONNECTION    ":$"                     "};for(int J=0;J<ƥ.Length;J++){ş.AppendLine(ƥ[J]);}var Ƥ=ş.ToString();foreach(var Ƒ in ű){var ƣ=
Ƒ.DrawFrame();var Ƣ=Color.Black;var ơ=DEFAULT_FONT_COLOR;switch(ŉ.Ķ){case ę.ė:Ƣ=Color.Yellow;ơ=Color.Black;break;case ę.Ė
:Ƣ=Color.Red;ơ=Color.White;break;case ę.ĕ:Ƣ=Color.Blue;ơ=Color.White;break;}Ƒ.BackgroundColor=Ƣ;Ƒ.ScriptBackgroundColor=Ƣ
;Ƒ.FontColor=ơ;var Ơ=Ƒ.SurfaceSize.X/256;var Ɵ=Ơ*0.6f;var ƞ=MySprite.CreateText(Ƥ,"Monospace",ơ,Ɵ);ƞ.Position=new Vector2
(128*Ơ,18*Ơ);ƣ.Add(ƞ);var Ɲ=MathHelper.Clamp(Math.Round(æ/(100f/15)),0,100);for(int J=0;J<Ɲ;J++){var Ɯ=8f*Ơ;var ƛ=2f*Ơ;
var ƚ=(Ƒ.SurfaceSize.X/2)-((15f*Ɯ+14f*ƛ)/2)+Ɯ/2;var K=new Vector2(ƚ+(Ɯ+ƛ)*J,Ɯ+ƛ);var ƫ=new Vector2(Ɯ);var ƭ=MySprite.
CreateSprite("Circle",K,ƫ);if(J<5){ƭ.Color=Color.Lime;}else if(J<10){ƭ.Color=Color.Red;}else{ƭ.Color=Color.Blue;}ƣ.Add(ƭ);}var ǀ=
MySprite.CreateSprite("Circle",new Vector2(22,12+8+4),new Vector2(22));ǀ.Color=ń.ô;var ƴ=MySprite.CreateSprite("Circle",new
Vector2(22,12+8+4),new Vector2(20));ƴ.Color=Color.Black;var ƿ=MySprite.CreateText(ń.Ĥ.ToString(),"DEBUG",Color.White,0.5f*Ơ);ƿ.
Position=new Vector2(22,12+4);var ƾ=MySprite.CreateSprite("SquareSimple",new Vector2(22,106),new Vector2(18,128));ƾ.Color=new
Color(32,32,32);var ƽ=ƾ.Size.GetValueOrDefault().Y;var Ƽ=ƽ*(ń.ã/100);var ƻ=MySprite.CreateSprite("SquareSimple",new Vector2(
22,42+(ƽ-Ƽ/2)),new Vector2(18,Ƽ));ƻ.Color=ń.ô;var ƺ=MySprite.CreateText($"{Math.Floor(ń.ã)}%","DEBUG",Color.White,Ɵ);ƺ.
Position=new Vector2(22,176);ǀ.Position*=Ơ;ǀ.Size*=Ơ;ƣ.Add(ǀ);ƴ.Position*=Ơ;ƴ.Size*=Ơ;ƣ.Add(ƴ);ƿ.Position*=Ơ;ƿ.Size*=Ơ;ƣ.Add(ƿ);
ƾ.Position*=Ơ;ƾ.Size*=Ơ;ƣ.Add(ƾ);ƻ.Position*=Ơ;ƻ.Size*=Ơ;ƣ.Add(ƻ);ƺ.Position*=Ơ;ƺ.Size*=Ơ;ƣ.Add(ƺ);ǀ=MySprite.
CreateSprite("IconEnergy",new Vector2(256-22,12+8+4),new Vector2(24));ǀ.Color=Color.Cyan;ƾ=MySprite.CreateSprite("SquareSimple",new
Vector2(256-22,106),new Vector2(18,128));ƾ.Color=new Color(32,32,32);ƽ=ƾ.Size.GetValueOrDefault().Y;Ƽ=ƽ*ľ;ƻ=MySprite.
CreateSprite("SquareSimple",new Vector2(256-22,42+(ƽ-Ƽ/2)),new Vector2(18,Ƽ));ƻ.Color=Color.Cyan;ƺ=MySprite.CreateText(
$"{Math.Floor(ľ*100)}%","DEBUG",Color.White,Ɵ);ƺ.Position=new Vector2(256-22,176);ǀ.Position*=Ơ;ǀ.Size*=Ơ;ƣ.Add(ǀ);ƾ.Position*=Ơ;ƾ.Size*=Ơ;ƣ.
Add(ƾ);ƻ.Position*=Ơ;ƻ.Size*=Ơ;ƣ.Add(ƻ);ƺ.Position*=Ơ;ƺ.Size*=Ơ;ƣ.Add(ƺ);var ƹ=MySprite.CreateText($"{ƨ}","Monospace",Z(ŉ.İ
),0.6f);ƹ.Position=new Vector2(128-64+4+1,128-4-2);var Ƹ=MySprite.CreateText($"{Ƨ}","Monospace",Z(ŉ.į),0.6f);Ƹ.Position=
new Vector2(128,128-4-2);var Ʒ=MySprite.CreateText($"{Ʀ}","Monospace",Z(ŉ.Į),0.6f);Ʒ.Position=new Vector2(128+64-4-2,128-4-
2);ƹ.Position*=Ơ;ƹ.Size*=Ơ;ƣ.Add(ƹ);Ƹ.Position*=Ơ;Ƹ.Size*=Ơ;ƣ.Add(Ƹ);Ʒ.Position*=Ơ;Ʒ.Size*=Ơ;ƣ.Add(Ʒ);ƣ.Dispose();}ş.
Clear();ƥ[6]=$"    {ƨ}  {Ƨ}  {Ʀ}    ";var ƶ=Math.Max(ƥ.Length,Math.Max(ō.Length,R.Length));for(int J=0;J<ƶ;J++){var Ƶ=J<ō.
Length?ō[J]:"   ";var ƴ=J<ƥ.Length?ƥ[J].Substring(3,15):"               ";var Ƴ=J<R.Length?R[J]:"   ";ş.AppendLine(
$"{Ƶ}{ƴ}{Ƴ}");}Ƥ=ş.ToString();ů?.WriteText(Ƥ);Ű?.WriteText(Ƥ);if(Ů!=null&&ŉ!=null&&ŉ.ı!=null){Ů.WriteText(ŉ.ı);}}void Ʋ(){if(!Ũ){
foreach(var Ô in ų){Ô.Power=ť;Ô.SetValueFloat("Speed Limit",ż*3.6f);}return;}foreach(var Ô in ų){Ô.Power=20f;Ô.SetValueFloat(
"Speed Limit",26f*3.6f);}var æ=Ų.GetShipSpeed();Ų.HandBrake=æ>24;}void Ʊ(){var ư=Ų.MoveIndicator.Z>0||Ų.MoveIndicator.Y>0||Ų.
HandBrake;if(ư){ŧ=false;}var Ư=B(ī.Ī);var Ʈ=B(ī.ĩ);var ƙ=B(ī.Ĩ);var Ƙ=B(ī.ħ);var ſ=(!ŧ?-150f:150f)*ň;Ư.Strength=MathHelper.Clamp(
Ư.Strength+ſ,DEFAULT_SUSPENSION_STRENGTH_F,100);Ʈ.Strength=MathHelper.Clamp(Ʈ.Strength+ſ,DEFAULT_SUSPENSION_STRENGTH_F,
100);ƙ.Strength=MathHelper.Clamp(ƙ.Strength+ſ,DEFAULT_SUSPENSION_STRENGTH_R,100);Ƙ.Strength=MathHelper.Clamp(Ƙ.Strength+ſ,
DEFAULT_SUSPENSION_STRENGTH_R,100);foreach(var Å in ņ){Å.Color=ŧ?Color.Blue:Color.Black;Å.Enabled=ŧ;}}void Ƌ(){if(Ũ){Ŧ=false;}var Ɗ=Ų.MoveIndicator.Z
<0;var æ=Ų.GetShipSpeed();const float Ɖ=1f/135;const float ƈ=1f/45;if(æ>=1){if(!Ŧ||(Ŧ&&!Ɗ)){var H=(float)MathHelper.Clamp
(æ/ż,0f,1f);ľ+=Ɖ*H*ň;}else{var H=1;ľ-=ƈ*H*ň;}}ľ=MathHelper.Clamp(ľ,0,1);if(ľ<=0){Ŧ=false;}foreach(var Ô in ų){Ô.Power=D()
;}var Ƃ=C();Ö(Ƃ);var Ƈ=Ŧ&&Ɗ?ŵ:0;t(Ƈ);foreach(var Å in Ň){Å.Color=Ŧ?Color.Cyan:Color.Black;}}void Ɔ(){ń.í(Ų,ų,ŝ,Ņ,ŉ,ň);Ó()
;}void ƅ(){if(ŭ==null){return;}ŭ.HudText=$"P{ŉ.Ď}";}void Ƅ(){Ĺ.Clear();if(Ŭ==null||Ŭ.Closed){Ľ=false;return;}Ŭ.
DetectedEntities(Ĺ);var ƃ=Ĺ.Any(h=>!h.IsEmpty()&&h.Type==MyDetectedEntityType.SmallGrid&&!h.Name.Contains("Grid")&&h.Velocity.Length()>=
70);if(ƃ){ļ=Ť;}if(Ũ){ļ=0;}Ľ=ļ>0;foreach(var Ô in ų){Ô.Power=D();var Ƃ=C();Ô.SetValueFloat("Speed Limit",Ƃ*3.6f);}ļ-=(int)(
ň*1000);}void Ɓ(){if(DRIVER_NUMBER<=0&&DRIVER_NUMBER>99){throw new Exception("DRIVER_NUMBER should be between 1 and 99");
}var ƀ=TEAM_TAG;if(TEAM_TAG==string.Empty){ƀ="XXX";}ƀ=ƀ.Trim().Substring(0,3).ToUpper();Me.CubeGrid.CustomName=
$"{ƀ} #{DRIVER_NUMBER:00}-{DRIVER_NAME.Trim()}";}void ƌ(){var ƍ=new List<IMyShipController>();GridTerminalSystem.GetBlocksOfType(ƍ);var Ɨ=ƍ.FirstOrDefault(Ɩ=>Ɩ is
IMyRemoteControl)??ƍ.FirstOrDefault(Ɩ=>Ɩ is IMyCockpit);if(Ɨ==null){throw new Exception("No cockpit or remote control!");}Ų=Ɨ;}void ƕ(){
var ë=new List<IMyMotorSuspension>();GridTerminalSystem.GetBlocksOfType(ë,Ô=>Ô.CubeGrid==Me.CubeGrid);if(ë==null||ë.Count!=
4){throw new Exception("Need 4 suspensions!");}var q=ë.FirstOrDefault(Ô=>Ô.CustomName.Contains("FL"));if(q==null)throw
new Exception("FL suspension not found.");var p=ë.FirstOrDefault(Ô=>Ô.CustomName.Contains("FR"));if(p==null)throw new
Exception("FR suspension not found.");var o=ë.FirstOrDefault(Ô=>Ô.CustomName.Contains("RL"));if(o==null)throw new Exception(
"RL suspension not found.");var n=ë.FirstOrDefault(Ô=>Ô.CustomName.Contains("RR"));if(n==null)throw new Exception("RR suspension not found.");ų=
new IMyMotorSuspension[4];A(ī.ĩ,q);A(ī.Ī,p);A(ī.ħ,o);A(ī.Ĩ,n);}void Ɣ(){ş=new StringBuilder();ű=new List<IMyTextSurface>{Me
.GetSurface(0)};var Ɠ=(IMyTextSurface)GridTerminalSystem.GetBlockWithName(DISPLAY_NAME);if(Ɠ!=null){ű.Add(Ɠ);}foreach(var
Ƒ in ű){Ƒ.ContentType=ContentType.SCRIPT;Ƒ.Alignment=TextAlignment.CENTER;Ƒ.Script=string.Empty;}if(COCKPIT_DISPLAY_INDEX
.HasValue){var ƒ=Ų as IMyCockpit;if(ƒ!=null){var Ƒ=ƒ.GetSurface(COCKPIT_DISPLAY_INDEX.GetValueOrDefault());if(Ƒ!=null){Ƒ.
WriteText(string.Empty);Ƒ.ContentType=ContentType.TEXT_AND_IMAGE;Ƒ.Alignment=TextAlignment.CENTER;Ƒ.Font="Monospace";Ű=Ƒ;}}}var Ɛ
=(IMyTextSurface)GridTerminalSystem.GetBlockWithName(TEXT_DISPLAY_NAME);if(Ɛ!=null){Ɛ.WriteText(string.Empty);Ɛ.
ContentType=ContentType.TEXT_AND_IMAGE;Ɛ.Alignment=TextAlignment.CENTER;Ɛ.Font="Monospace";((IMyTerminalBlock)Ɛ).CustomData=
TEXT_DISPLAY_HUDLCD;ů=Ɛ;}var Ə=(IMyTextSurface)GridTerminalSystem.GetBlockWithName(RANK_DISPLAY_NAME);if(Ə!=null){Ə.WriteText(string.Empty)
;Ə.ContentType=ContentType.TEXT_AND_IMAGE;Ə.Alignment=TextAlignment.CENTER;Ə.Font="Monospace";((IMyTerminalBlock)Ə).
CustomData=RANK_DISPLAY_HUDLCD;Ů=Ə;}}void Ǝ(){var È=new List<IMyLightingBlock>();GridTerminalSystem.GetBlockGroupWithName(
BRAKELIGHT_GROUP_NAME).GetBlocksOfType<IMyLightingBlock>(È,Æ=>Æ.CubeGrid==Me.CubeGrid);if(È.Count<=0){throw new Exception(
$"\"{BRAKELIGHT_GROUP_NAME}\" group not set.");}ŝ=new List<IMyLightingBlock>();foreach(var Å in È){Å.Intensity=5f;Å.BlinkLength=50f;Å.BlinkIntervalSeconds=0f;ŝ.Add(Å
);}Ņ=new List<IMyLightingBlock>();GridTerminalSystem.GetBlocksOfType(Ņ,Æ=>Æ.CubeGrid!=Me.CubeGrid);foreach(var Å in Ņ){Å.
BlinkLength=50f;Å.BlinkIntervalSeconds=0;}}void Ê(){ņ=new List<IMyLightingBlock>();var È=new List<IMyTerminalBlock>();var Ç=
GridTerminalSystem.GetBlockGroupWithName(DRS_LIGHTS_GROUP_NAME);if(Ç==null){return;}Ç.GetBlocks(È,Æ=>Æ.CubeGrid==Me.CubeGrid);foreach(var
Å in È){var Ä=(IMyLightingBlock)Å;ņ.Add(Ä);}}void É(){Ň=new List<IMyLightingBlock>();var È=new List<IMyTerminalBlock>();
var Ç=GridTerminalSystem.GetBlockGroupWithName(ERS_LIGHTS_GROUP_NAME);if(Ç==null){return;}Ç.GetBlocks(È,Æ=>Æ.CubeGrid==Me.
CubeGrid);foreach(var Å in È){var Ä=(IMyLightingBlock)Å;Ä.Radius=4f;Ä.Intensity=10f;Ä.BlinkLength=50f;Ä.BlinkIntervalSeconds=
0.5f;Ň.Add(Ä);}}void Ã(){var Â=(IMySensorBlock)GridTerminalSystem.GetBlockWithName(DRAFTING_SENSOR_NAME);if(Â==null){return;
}Ŭ=Â;Ŭ.TopExtend=50;Ŭ.BottomExtend=0;Ŭ.RightExtend=2.25f;Ŭ.LeftExtend=2.25f;Ŭ.FrontExtend=0;Ŭ.BackExtend=2;Ŭ.
DetectSmallShips=true;Ŭ.DetectLargeShips=false;Ŭ.DetectStations=false;Ŭ.DetectSubgrids=false;Ŭ.DetectAsteroids=false;Ŭ.DetectPlayers=
false;Ĺ=new List<MyDetectedEntityInfo>();}void Á(){var À=new List<IMyGyro>();GridTerminalSystem.GetBlocksOfType(À,h=>h.
CubeGrid==Me.CubeGrid);if(À.Count<=0){throw new Exception("No gyroscope found.");}ũ=À;}void º(){if(string.IsNullOrWhiteSpace(Me.
CustomData)){Ú(ā.ÿ);return;}var µ=Me.CustomData.Split(';');if(µ.Length<3){Ú(ā.ÿ);return;}var ª=Convert.ToChar(µ[0]);var z=(float)
Convert.ToDouble(µ[1]);var Ë=(float)Convert.ToDouble(µ[2]);switch(ª){case'U':Ú(ā.Ā);break;case'S':Ú(ā.ÿ);break;case'M':Ú(ā.þ);
break;case'H':Ú(ā.ý);break;case'X':Ú(ā.ü);break;case'I':Ú(ā.û);break;case'W':Ú(ā.ú);break;default:Ú(ā.ÿ);break;}ń.ģ=z;ľ=Ë;}
void Í(){var à=new List<IMyRadioAntenna>();GridTerminalSystem.GetBlocksOfType(à);var á=à.FirstOrDefault();if(á==null){throw
new Exception("No antenna!");}á.Enabled=true;á.Radius=5000;á.EnableBroadcasting=true;á.HudText=
$"(P{ŉ.Ď}) {DRIVER_NAME}-{DRIVER_NUMBER}";ŭ=á;}void ß(){IGC.RegisterBroadcastListener("Address");var Þ=new List<IMyBroadcastListener>();IGC.GetBroadcastListeners
(Þ);ł=Þ.FirstOrDefault();}void Ý(string Ü){if(Ü.Equals("LMT",StringComparison.InvariantCultureIgnoreCase)){Ũ=!Ũ;return;}
if(Ü.Equals("LMT_ON",StringComparison.InvariantCultureIgnoreCase)){Ũ=true;return;}if(Ü.Equals("LMT_OFF",StringComparison.
InvariantCultureIgnoreCase)){Ũ=false;return;}if(Ü.Equals("DRS",StringComparison.InvariantCultureIgnoreCase)){ŧ=!ŧ;return;}if(Ü.Equals("DRS_ON",
StringComparison.InvariantCultureIgnoreCase)){ŧ=true;return;}if(Ü.Equals("DRS_OFF",StringComparison.InvariantCultureIgnoreCase)){ŧ=false
;return;}if(Ü.Equals("ERS",StringComparison.InvariantCultureIgnoreCase)){Ŧ=!Ŧ;return;}if(Ü.Equals("ERS_ON",
StringComparison.InvariantCultureIgnoreCase)){Ŧ=true;return;}if(Ü.Equals("ERS_OFF",StringComparison.InvariantCultureIgnoreCase)){Ŧ=false
;return;}if(Ü.Equals("ULTRA",StringComparison.InvariantCultureIgnoreCase)){Û(ā.Ā);return;}if(Ü.Equals("SOFT",
StringComparison.InvariantCultureIgnoreCase)){Û(ā.ÿ);return;}if(Ü.Equals("MEDIUM",StringComparison.InvariantCultureIgnoreCase)){Û(ā.þ);
return;}if(Ü.Equals("HARD",StringComparison.InvariantCultureIgnoreCase)){Û(ā.ý);return;}if(Ü.Equals("EXTRA",StringComparison.
InvariantCultureIgnoreCase)){Û(ā.ü);return;}if(Ü.Equals("INT",StringComparison.InvariantCultureIgnoreCase)){Û(ā.û);return;}if(Ü.Equals("WET",
StringComparison.InvariantCultureIgnoreCase)){Û(ā.ú);return;}if(Ü.Equals("FLIP",StringComparison.InvariantCultureIgnoreCase)){Ļ=true;
return;}if(Ü.Equals("FLAG_G",StringComparison.InvariantCultureIgnoreCase)){F(ę.Ę);return;}if(Ü.Equals("FLAG_Y",
StringComparison.InvariantCultureIgnoreCase)){F(ę.ė);return;}if(Ü.Equals("FLAG_R",StringComparison.InvariantCultureIgnoreCase)){F(ę.Ė);
return;}}void Û(ā Ù){if(!Ũ||Ų.GetShipSpeed()>1){return;}Ú(Ù);Ó(true);}void Ú(ā Ù){switch(Ù){case ā.Ā:ń=Ħ.Ĉ();break;case ā.ÿ:ń=
Ħ.ć();break;case ā.þ:ń=Ħ.Ć();break;case ā.ý:ń=Ħ.ą();break;case ā.ü:ń=Ħ.Ą();break;case ā.û:ń=Ħ.ă();break;case ā.ú:ń=Ħ.Ă();
break;default:break;}Ø(ń.ô);foreach(var Ô in ų){Ô.ApplyAction("Add Top Part");Ô.Friction=ń.Ģ;}}void Ø(Color w){foreach(var Å
in ŝ){Å.Color=w;Å.BlinkIntervalSeconds=0;}foreach(var Å in Ņ){if(Å.IsSameConstructAs(Me)){Å.Color=w;Å.BlinkIntervalSeconds
=0;}}}void Ö(float Õ){foreach(var Ô in ų){Ô.SetValueFloat("Speed Limit",Õ*3.6f);}}void Ó(bool Ò=false){ŀ-=(int)(ň*1000);
if(!Ò&&ŀ>0){return;}var Ñ=ń.Ĥ;Me.CustomData=$"{Ñ};{ń.ģ};{ľ}";ŀ=š;}string Ð(){var Ï=string.Empty;switch(ŉ.Ķ){case ę.ĕ:Ï=
"Blue";break;case ę.Ę:Ï="Green";break;case ę.Ė:Ï="Red";break;case ę.ė:Ï="Yellow";break;}return Ï;}Color Î(){var w=Color.Black;
switch(ŉ.Ķ){case ę.ĕ:w=Color.Blue;break;case ę.Ę:w=Color.Green;break;case ę.Ė:w=Color.Red;break;case ę.ė:w=Color.Yellow;break;
}return w;}string U(){const int u=6;var S=Ŧ?Ž:ľ<1?Ż:'-';var R=S+"E:";for(int J=0;J<u;J++){var H=1f/u;if(ľ>H*J){if(ľ<H*(J+
1)){R+=ŷ;continue;}R+=Ÿ;}else{R+=Ŷ;}}return R;}string[]Q(char P,int O,float N,float M){var T=new string[O+2];T[0]=
$"┌{P}┐";var L=Math.Floor(100*N/M);for(int J=1;J<T.Length-1;J++){var I=J-1;var H=100f/O;var G=T.Length-1-J;if(L>H*I){if(L<H*(I+1
)){T[G]=$"│{ŷ}│";continue;}T[G]=$"│{Ÿ}│";}else{T[G]=$"│{Ŷ}│";}}T[T.Length-1]=L<100?$"{L+"%",3}":$"{L}";return T;}void F(ę
E){if(Ń<=0){return;}IGC.SendUnicastMessage(Ń,"Flag",$"{(int)E}");}float D(){if(Ũ){return 20f;}if(Ľ||Ŧ){return 100f;}
return ť;}float C(){if(Ũ){return 26;}if(ŉ.Ķ==ę.ė){return 45;}if(Ľ){return 999;}if(Ŧ){return 98f;}return ż;}IMyMotorSuspension
B(ī K){return ų[(int)K];}void A(ī K,IMyMotorSuspension v){if(v==null){return;}ų[(int)K]=v;}void t(float r){var q=B(ī.ĩ);
var p=B(ī.Ī);var o=B(ī.ħ);var n=B(ī.Ĩ);q.PropulsionOverride=r;p.PropulsionOverride=-r;o.PropulsionOverride=r;n.
PropulsionOverride=-r;}float m(IMySensorBlock k){if(k==null||k.Closed||!k.IsActive){return float.MaxValue;}ĺ.Clear();k.DetectedEntities(ĺ)
;if(ĺ.Count<=0){return float.MaxValue;}var K=Me.CubeGrid.GetPosition();var j=ĺ.Select(h=>Vector3.Distance(K,h.Position)).
Min();return j;}string g(bool f){var e=m(f?ū:Ū);if(e==float.MaxValue){return string.Empty;}var S=f?ź:Ź;if(e<15){return
$"{S}{S}{S}";}if(e<30){return$"{S}{S}";}return$"{S}";}char a(Ĕ Y){switch(Y){case Ĕ.ē:return ĸ.â;case Ĕ.Ē:return'-';case Ĕ.đ:return
'+';case Ĕ.Đ:return'*';default:return ĸ.â;}}Color Z(Ĕ Y){switch(Y){case Ĕ.ē:return Color.White;case Ĕ.Ē:return Color.Yellow
;case Ĕ.đ:return Color.Lime;case Ĕ.Đ:return Color.Magenta;default:return Color.White;}}class X{private char[]W;private
int y;public int V{get;private set;}public char â{get{return W[V];}}public int ğ{get;}public int ĝ{get;set;}public X(char[]
Ĝ,int ě){W=Ĝ;ğ=Ĝ.Length;ĝ=ě;}public void í(float ç){var Ě=(int)(ç*1000);y+=Ě;if(y>=ĝ){V++;if(V==ğ){V=0;}y-=ĝ;}}public
override string ToString(){return$"{â}";}}private enum ę{Ę,ė,Ė,ĕ}private enum Ĕ{ē,Ē,đ,Đ}class ď{public int Ď{get;set;}public int
č{get;set;}public int Č{get;set;}public int Ğ{get;set;}public string Ġ{get;set;}="--:--.---";public string ĵ{get;set;}=
"--:--.---";public ę Ķ{get;set;}public ù Ĵ{get;set;}public int ĳ{get;set;}public int Ĳ{get;set;}public string ı{get;set;}public Ĕ İ
{get;set;}public Ĕ į{get;set;}public Ĕ Į{get;set;}public string ĭ{get;set;}="--:--.---";public void Ĭ(string è){try{var µ
=è.Split(';');Č=Convert.ToInt32(µ[0]);Ď=Convert.ToInt32(µ[1]);Ġ=µ[2];ĵ=µ[3];č=Convert.ToInt32(µ[4]);Ğ=Convert.ToInt32(µ[5
]);Ķ=(ę)Convert.ToInt32(µ[6]);Ĵ=(ù)Convert.ToInt32(µ[7]);ĳ=Convert.ToInt32(µ[8]);Ĳ=Convert.ToInt32(µ[9]);ı=µ[10];İ=(Ĕ)
Convert.ToInt32(µ[11]);į=(Ĕ)Convert.ToInt32(µ[12]);Į=(Ĕ)Convert.ToInt32(µ[13]);ĭ=µ[14];}catch(Exception){}}}private enum ī{Ī,ĩ,
Ĩ,ħ}class Ħ{private float ĥ;public char Ĥ{get;private set;}public float ģ{get;set;}public float Ģ{get;private set;}public
float ġ{get;private set;}public float ċ{get;private set;}public float Ċ{get;private set;}public float ã{get{return((ģ-ġ)/(Ģ-ġ
))*100f;}}public int ö{get;private set;}public bool õ{get;private set;}public Color ô{get;private set;}private Ħ(float ó,
int ò,float ñ,float ð,char ï,Color w,bool î=true){Ģ=ó;ģ=Ģ;ö=ò;ċ=ñ;Ċ=ð;ġ=(float)Math.Round(ċ-((Ģ-ċ)/(100-Ċ))*Ċ,2);Ĥ=ï;ô=w;õ=
î;ĥ=(Ģ-ġ)/(60*ö);}public void í(IMyShipController ì,IMyMotorSuspension[]ë,List<IMyLightingBlock>ê,List<IMyLightingBlock>é
,ď è,float ç){var æ=ì.GetShipSpeed();if(æ<1){return;}var å=(float)MathHelper.Clamp(æ,0,90)/90;var ä=ĥ*å*ç;ģ-=ä;ģ=
MathHelper.Clamp(ģ,ġ,Ģ);foreach(var Ô in ë){Ô.Friction=!(õ&&è.Ĵ==ù.ø)?ģ:ģ/2;}if(ã<=Ċ){if(é.Any(Å=>Å.BlinkIntervalSeconds<=0)){
foreach(var Å in ê){Å.BlinkIntervalSeconds=0.25f;}foreach(var Å in é){Å.BlinkIntervalSeconds=0.25f;}}}else{if(é.Any(Å=>Å.
BlinkIntervalSeconds>0)){foreach(var Å in ê){Å.BlinkIntervalSeconds=0f;}foreach(var Å in é){Å.BlinkIntervalSeconds=0f;}}}if(ģ<=ġ){if(ë.All(Ô
=>Ô.IsAttached)){var ĉ=new Random().Next(4);ë[ĉ].Detach();}}}public static Ħ Ĉ(){return new Ħ(100,5,60,20,'U',new Color(
192,0,255));}public static Ħ ć(){return new Ħ(100,8,50,20,'S',Color.Red);}public static Ħ Ć(){return new Ħ(75,13,50,20,'M',
Color.Yellow);}public static Ħ ą(){return new Ħ(60,21,50,20,'H',Color.White);}public static Ħ Ą(){return new Ħ(55,34,50,20,
'X',new Color(255,32,0));}public static Ħ ă(){return new Ħ(60,8,40,10,'I',Color.Green,false);}public static Ħ Ă(){return
new Ħ(50,21,40,10,'W',new Color(0,16,255),false);}}private enum ā{Ā,ÿ,þ,ý,ü,û,ú}private enum ù{Ì,ø}