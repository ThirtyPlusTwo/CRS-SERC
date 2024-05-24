
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
    
//************ DO NOT MODIFY BELLOW HERE ************

private readonly string CODE_VERSION = "13.1.0";
private const int CONNECTION_TIMEOUT = 3000;
private const int SAVE_STATE_COOLDOWN = 1000;
private const int DRAFTING_COOLDOWN = 1000;
private const float DEFAULT_SUSPENSION_POWER = 80f;
private const float DEFAULT_SUSPENSION_SPEED_LIMIT = 95f;
private readonly char ARROW_DOWN_CHAR = '\u25BC';
private readonly char ARROW_UP_CHAR = '\u25B2';
private readonly char ARROW_RIGHT = '\u25BA';
private readonly char ARROW_LEFT = '\u25C4';
private const char BLOCK_FILLED_CHAR = '\u2588';
private const char BLOCK_HALF_CHAR = '\u2592';
private const char BLOCK_EMPTY_CHAR = '\u2591';
private const float ERS_PROPULSION_OVERRIDE = 1.7f;
private bool _hasError;
private IMyMotorSuspension[] _suspensions;
private IMyShipController _mainController;
private List<IMyTextSurface> _displays;
private IMyTextSurface _cockpitDisplay;
private IMyTextSurface _textDisplay;
private IMyTextSurface _rankDisplay;
private IMyRadioAntenna _antenna;
private IMySensorBlock _draftingSensor;
private IMySensorBlock _mirrorRight;
private IMySensorBlock _mirrorLeft;
private List<IMyGyro> _gyros;
private bool _isPitLimiterActive;
private bool _isDrsActive;
private bool _isErsActive;
private StringBuilder _stringBuilder;
private RaceData _data;
private List<IMyLightingBlock> _brakelights;
private List<IMyLightingBlock> _ersLights;
private List<IMyLightingBlock> _drsLights;
private List<IMyLightingBlock> _tyreLights;
private Tyre _currentTyres;
private long _address = -1;
private IMyBroadcastListener _broadcastListener;
private int _connectionTimeout;
private int _saveStateCooldown;
private DateTime _lastTimeStamp;
private float _delta;
private float _ersCharge = 1f;
private bool _isDrafting = false;
private int _draftingCooldown;
private bool _doFlip;
private List<MyDetectedEntityInfo> _mirrorAuxList;
private List<MyDetectedEntityInfo> _draftingAuxList;
private CharacterAnimation _spinnerAnim;

public Program()
{
    _data = new RaceData();
    _spinnerAnim = new CharacterAnimation(new char[] { '-', '\\', '|', '/' }, 150);

    try
    {
        SetupGridName();
        SetupController();
        SetupSuspensions();
        SetupDisplays();
        SetupBrakelights();
        SetupErsLights();
        SetupDrsLights();
        SetupAntenna();
        SetupMirrors();
        LoadState();
        SetupBroadcastListener();
        SetupDraftingSensor();
        SetupGyros();
    }
    catch (Exception ex)
    {
        _hasError = true;
        Echo(ex.Message);
        return;
    }

    Runtime.UpdateFrequency = UpdateFrequency.Update1;
    _lastTimeStamp = DateTime.Now;
}

private void SetupMirrors()
{
    _mirrorAuxList = new List<MyDetectedEntityInfo>();
    _mirrorRight = (IMySensorBlock)GridTerminalSystem.GetBlockWithName(MIRROR_SENSOR_RIGHT_NAME);

    if (_mirrorRight != null)
    {
        _mirrorRight.DetectSmallShips = true;
        _mirrorRight.DetectLargeShips = false;
        _mirrorRight.DetectPlayers = false;
        _mirrorRight.LeftExtend = 12.5f;
        _mirrorRight.RightExtend = 0.5f;
        _mirrorRight.FrontExtend = 50;
        _mirrorRight.BackExtend = 5;
        _mirrorRight.TopExtend = 5;
        _mirrorRight.BottomExtend = 5;
    }

    _mirrorLeft = (IMySensorBlock)GridTerminalSystem.GetBlockWithName(MIRROR_SENSOR_LEFT_NAME);

    if (_mirrorLeft != null)
    {
        _mirrorLeft.DetectSmallShips = true;
        _mirrorLeft.DetectLargeShips = false;
        _mirrorLeft.DetectPlayers = false;
        _mirrorLeft.LeftExtend = 0.5f;
        _mirrorLeft.RightExtend = 12.5f;
        _mirrorLeft.FrontExtend = 50;
        _mirrorLeft.BackExtend = 5;
        _mirrorLeft.TopExtend = 5;
        _mirrorLeft.BottomExtend = 5;
    }
}

public void Save()
{
    // Called when the program needs to save its state. Use
    // this method to save your state to the Storage field
    // or some other means. 
    // 
    // This method is optional and can be removed if not
    // needed.
}

public void Main(string argument, UpdateType updateSource)
{
    if (_hasError)
    {
        return;
    }

    var currentTimeStamp = DateTime.Now;
    _delta = (float)(currentTimeStamp - _lastTimeStamp).TotalMilliseconds / 1000;

    Echo($"Running CRS-F1 {CODE_VERSION}");

    HandleArgument(argument);
    UpdateDrs();
    UpdateErs();
    UpdateFlagEffect();
    UpdatePitLimiter();
    UpdateDraftingSensor();
    UpdateTyreDegradation();
    UpdateCommunication();
    UpdateDisplays();
    UpdateAntenna();
    UpdateGyros();

    _lastTimeStamp = currentTimeStamp;
}

private void UpdateGyros()
{
    if (!_doFlip)
    {
        return;
    }

    // The dot product is only positive when the car is flipped over
    if (Vector3D.Dot(_mainController.GetNaturalGravity(), _mainController.WorldMatrix.Up) > 0)
    {
        for (int i = 0; i < _gyros.Count; i++)
        {
            _gyros[i].GyroOverride = true;
        }

        return;
    }

    // Simply disables gyro override if it's on
    if (_gyros[0].GyroOverride)
    {
        for (int i = 0; i < _gyros.Count; i++)
        {
            _gyros[i].GyroOverride = false;
        }

        _doFlip = false;
    }
}

private void UpdateFlagEffect()
{
    switch (_data.CurrentFlag)
    {
        case Flag.Yellow:
            UpdateYellowFlagEffect();
            break;

        case Flag.Red:
            UpdateRedFlagEffect();
            break;

        default:
            if (!_isPitLimiterActive)
            {
                _mainController.HandBrake = false;
                SetSpeedLimit(DEFAULT_SUSPENSION_SPEED_LIMIT);
            }
            break;
    }
}

private void UpdateYellowFlagEffect()
{
    _isDrsActive = false;
    _isErsActive = false;
    _mainController.HandBrake = _mainController.GetShipSpeed() > 50;

    SetSpeedLimit(50f);
}

private void UpdateRedFlagEffect()
{
    _isDrsActive = false;
    _isErsActive = false;
    _isPitLimiterActive = false;
    _mainController.HandBrake = true;
}

private void UpdateCommunication()
{
    var unisource = IGC.UnicastListener;

    if (!unisource.HasPendingMessage)
    {
        _connectionTimeout -= (int)(_delta * 1000);

        if (_broadcastListener.HasPendingMessage && _connectionTimeout <= 0)
        {
            var message = _broadcastListener.AcceptMessage();

            if (message.Tag == "Address")
            {
                _address = Convert.ToInt64(message.Data.ToString());
                IGC.SendUnicastMessage(_address, "Register", $"{Me.CubeGrid.CustomName};{IGC.Me}");
            }
        }

        return;
    }

    while (unisource.HasPendingMessage)
    {
        var messageUni = unisource.AcceptMessage();

        if (messageUni.Tag == "RaceData")
        {
            _data.Map(messageUni.Data.ToString());
        }

        if (messageUni.Tag == "Argument")
        {
            HandleArgument(messageUni.Data.ToString());
        }
    }

    _connectionTimeout = CONNECTION_TIMEOUT;
}

private void UpdateDisplays()
{
    _stringBuilder.Clear();

    const int DISPLAY_WIDTH = 21;
    const int INNER_DISPLAY_WIDTH = DISPLAY_WIDTH - 6;
    var speed = _mainController.GetShipSpeed();
    var tyreCompoundIndicator = _currentTyres.Symbol;
    var strSpeed = $"{Math.Floor(speed)}m/s";
    var ersBar = BuildVerticalBar('E', 8, _ersCharge, 1);
    var tyreBar = BuildVerticalBar(tyreCompoundIndicator, 8, _currentTyres.WearPercentage, 1);
    var strWeather = $"<{Weather.GetWeatherDescription(_data.CurrentWeather)}>".ToUpper();
    var strWeatherLevelSlider = BuildWeatherLevelSlider(_data.CurrentWeather);
    var strWeatherLine1 = $"DRY         WET";
    var strWeatherLine2 = CentralizeString(strWeather, INNER_DISPLAY_WIDTH);
    var controlLine = (_isDrsActive ? "DRS" : "   ") + " " +
        (_isErsActive ? "ERS" : "   ") + " " +
        (_isDrafting ? "DFT" : "   ") + " " +
        (_isPitLimiterActive ? "PIT" : "   ");

    var rightProximity = GetMirrorProximityArrows(true);
    var leftProximity = GetMirrorProximityArrows(false);
    var innerLine = leftProximity.PadRight((int)Math.Ceiling((float)INNER_DISPLAY_WIDTH / 2) - (int)Math.Ceiling((float)strSpeed.Length / 2))
        + strSpeed +
        rightProximity.PadLeft((int)Math.Floor((float)INNER_DISPLAY_WIDTH / 2) - (int)Math.Floor((float)strSpeed.Length / 2));

    _spinnerAnim.Update(_delta);

    var strS1 = $"S1{GetSectorStatusChar(_data.StatusS1)}";
    var strS2 = $"S2{GetSectorStatusChar(_data.StatusS2)}";
    var strS3 = $"S3{GetSectorStatusChar(_data.StatusS3)}";

    var lines = new string[]
    {
        $"   {innerLine}   ",
        $"   {controlLine}   ",
        $"   P:{_data.Position:00}/{_data.TotalRacers:00} L:{(_data.Laps):00}/{_data.TotalLaps:00}   ",
        $"   TIME: {_data.CurrentLapTime}   ",
        $"   BEST: {_data.BestLapTime}   ",
        $"   PREV: {_data.PrevLapTime}   ",
        $"                     ",
        $"   {strWeatherLine1}   ",
        $"   {strWeatherLine2}   ",
        (_connectionTimeout <= 0)
            ? $"    NO CONNECTION    "
            : $"                     "
    };

    for (int i = 0; i < lines.Length; i++)
    {
        _stringBuilder.AppendLine(lines[i]);
    }

    var text = _stringBuilder.ToString();

    foreach (var d in _displays)
    {
        var frame = d.DrawFrame();
        var bgColor = Color.Black;
        var fontColor = DEFAULT_FONT_COLOR;

        switch (_data.CurrentFlag)
        {
            case Flag.Yellow:
                bgColor = Color.Yellow;
                fontColor = Color.Black;
                break;

            case Flag.Red:
                bgColor = Color.Red;
                fontColor = Color.White;
                break;

            case Flag.Blue:
                bgColor = Color.Blue;
                fontColor = Color.White;
                break;
        }

        d.BackgroundColor = bgColor;
        d.ScriptBackgroundColor = bgColor;
        d.FontColor = fontColor;

        var scale = d.SurfaceSize.X / 256;
        var textScale = scale * 0.6f;
        var textSprite = MySprite.CreateText(text, "Monospace", fontColor, textScale);
        textSprite.Position = new Vector2(128 * scale, 18 * scale);
        frame.Add(textSprite);

        var dots = MathHelper.Clamp(Math.Round(speed / (100f / 15)), 0, 100);

        for (int i = 0; i < dots; i++)
        {
            var size = 8f * scale;
            var spacing = 2f * scale;
            var startAt = (d.SurfaceSize.X / 2) - ((15f * size + 14f * spacing) / 2) + size / 2;
            var pos = new Vector2(startAt + (size + spacing) * i, size + spacing);
            var dimensions = new Vector2(size);
            var circle = MySprite.CreateSprite("Circle", pos, dimensions);

            if (i < 5)
            {
                circle.Color = Color.Lime;
            }
            else if (i < 10)
            {
                circle.Color = Color.Red;
            }
            else
            {
                circle.Color = Color.Blue;
            }

            frame.Add(circle);
        }

        //Tyre Bar
        var icon = MySprite.CreateSprite("Circle", new Vector2(22, 12 + 8 + 4), new Vector2(22));
        icon.Color = _currentTyres.Color;

        var inner = MySprite.CreateSprite("Circle", new Vector2(22, 12 + 8 + 4), new Vector2(20));
        inner.Color = Color.Black;

        var tyreSymbol = MySprite.CreateText(_currentTyres.Symbol.ToString(), "DEBUG", Color.White, 0.5f * scale);
        tyreSymbol.Position = new Vector2(22, 12 + 4);

        var fillBg = MySprite.CreateSprite("SquareSimple", new Vector2(22, 106), new Vector2(18, 128));
        fillBg.Color = new Color(32, 32, 32);

        var totalHeight = fillBg.Size.GetValueOrDefault().Y;
        var height = totalHeight * _currentTyres.WearPercentage;

        var fill = MySprite.CreateSprite("SquareSimple", new Vector2(22, 42 + (totalHeight - height / 2)), new Vector2(18, height));
        fill.Color = _currentTyres.Color;

        var percText = MySprite.CreateText($"{Math.Floor(_currentTyres.WearPercentage * 100)}%", "DEBUG", Color.White, textScale);
        percText.Position = new Vector2(22, 176);

        icon.Position *= scale;
        icon.Size *= scale;
        frame.Add(icon);

        inner.Position *= scale;
        inner.Size *= scale;
        frame.Add(inner);

        tyreSymbol.Position *= scale;
        tyreSymbol.Size *= scale;
        frame.Add(tyreSymbol);

        fillBg.Position *= scale;
        fillBg.Size *= scale;
        frame.Add(fillBg);

        fill.Position *= scale;
        fill.Size *= scale;
        frame.Add(fill);

        percText.Position *= scale;
        percText.Size *= scale;
        frame.Add(percText);

        //ERS Bar
        icon = MySprite.CreateSprite("IconEnergy", new Vector2(256 - 22, 12 + 8 + 4), new Vector2(24));
        icon.Color = Color.Cyan;

        fillBg = MySprite.CreateSprite("SquareSimple", new Vector2(256 - 22, 106), new Vector2(18, 128));
        fillBg.Color = new Color(32, 32, 32);

        totalHeight = fillBg.Size.GetValueOrDefault().Y;
        height = totalHeight * _ersCharge;

        fill = MySprite.CreateSprite("SquareSimple", new Vector2(256 - 22, 42 + (totalHeight - height / 2)), new Vector2(18, height));
        fill.Color = Color.Cyan;

        percText = MySprite.CreateText($"{Math.Floor(_ersCharge * 100)}%", "DEBUG", Color.White, textScale);
        percText.Position = new Vector2(256 - 22, 176);

        icon.Position *= scale;
        icon.Size *= scale;
        frame.Add(icon);

        fillBg.Position *= scale;
        fillBg.Size *= scale;
        frame.Add(fillBg);

        fill.Position *= scale;
        fill.Size *= scale;
        frame.Add(fill);

        percText.Position *= scale;
        percText.Size *= scale;
        frame.Add(percText);

        //Sectors
        var textS1 = MySprite.CreateText($"{strS1}", "Monospace", GetSectorStatusColor(_data.StatusS1), 0.6f);
        textS1.Position = new Vector2(128 - 64 + 4 + 1, 128 - 4 - 2);

        var textS2 = MySprite.CreateText($"{strS2}", "Monospace", GetSectorStatusColor(_data.StatusS2), 0.6f);
        textS2.Position = new Vector2(128, 128 - 4 - 2);

        var textS3 = MySprite.CreateText($"{strS3}", "Monospace", GetSectorStatusColor(_data.StatusS3), 0.6f);
        textS3.Position = new Vector2(128 + 64 - 4 - 2, 128 - 4 - 2);

        textS1.Position *= scale;
        textS1.Size *= scale;
        frame.Add(textS1);

        textS2.Position *= scale;
        textS2.Size *= scale;
        frame.Add(textS2);

        textS3.Position *= scale;
        textS3.Size *= scale;
        frame.Add(textS3);

        for (int l = -3; l <= 3; l++)
        {
            var character = (int)_data.CurrentWeather != l ? "■" : "█";
            var t = MySprite.CreateText(character, "Monospace", Weather.GetWeatherColor((WeatherLevel)l), 0.6f);
            t.Position = new Vector2(128 - 64 + 32 - 5 + (l + 3) * 12, 128 + 11);

            t.Position *= scale;
            t.Size *= scale;
            frame.Add(t);
        }

        frame.Dispose();
    }

    _stringBuilder.Clear();
    lines[6] = $"    {strS1}  {strS2}  {strS3}    ";
    lines[7] = $"   DRY {strWeatherLevelSlider} WET   ";

    var maxLines = Math.Max(lines.Length, Math.Max(tyreBar.Length, ersBar.Length));

    for (int i = 0; i < maxLines; i++)
    {
        var prefix = i < tyreBar.Length ? tyreBar[i] : "   ";
        var inner = i < lines.Length ? lines[i].Substring(3, 15) : "               ";
        var sufix = i < ersBar.Length ? ersBar[i] : "   ";

        _stringBuilder.AppendLine($"{prefix}{inner}{sufix}");
    }

    text = _stringBuilder.ToString();
    _textDisplay?.WriteText(text);
    _cockpitDisplay?.WriteText(text);

    if (_rankDisplay != null && _data != null && _data.RankTable != null)
    {
        _rankDisplay.WriteText(_data.RankTable);
    }
}

private string BuildWeatherLevelSlider(WeatherLevel weatherLevel)
{
    var slider = string.Empty;
    var level = (int)weatherLevel;

    for (int i = -3; i <= 3; i++)
    {
        if (level == i)
        {
            slider += "█";
            continue;
        }

        slider += "■";
    }

    return slider;
}

private void UpdatePitLimiter()
{
    if (!_isPitLimiterActive)
    {
        foreach (var s in _suspensions)
        {
            s.Power = DEFAULT_SUSPENSION_POWER;
            s.SetValueFloat("Speed Limit", DEFAULT_SUSPENSION_SPEED_LIMIT * 3.6f);
        }

        return;
    }

    foreach (var s in _suspensions)
    {
        s.Power = 20f;
        s.SetValueFloat("Speed Limit", 26f * 3.6f);
    }

    var speed = _mainController.GetShipSpeed();
    _mainController.HandBrake = speed > 24;
}

private void UpdateDrs()
{
    var isBreaking = _mainController.MoveIndicator.Z > 0
        || _mainController.MoveIndicator.Y > 0
        || _mainController.HandBrake;

    if (isBreaking)
    {
        _isDrsActive = false;
    }

    var fr = GetSuspension(SuspensionPosition.FrontRight);
    var fl = GetSuspension(SuspensionPosition.FrontLeft);
    var rr = GetSuspension(SuspensionPosition.RearRight);
    var rl = GetSuspension(SuspensionPosition.RearLeft);
    var rate = (!_isDrsActive ? -150f : 150f) * _delta;

    fr.Strength = MathHelper.Clamp(fr.Strength + rate, DEFAULT_SUSPENSION_STRENGTH_F, 100);
    fl.Strength = MathHelper.Clamp(fl.Strength + rate, DEFAULT_SUSPENSION_STRENGTH_F, 100);
    rr.Strength = MathHelper.Clamp(rr.Strength + rate, DEFAULT_SUSPENSION_STRENGTH_R, 100);
    rl.Strength = MathHelper.Clamp(rl.Strength + rate, DEFAULT_SUSPENSION_STRENGTH_R, 100);

    foreach (var l in _drsLights)
    {
        l.Color = _isDrsActive ? Color.Blue : Color.Black;
        l.Enabled = _isDrsActive;
    }
}

private void UpdateErs()
{
    if (_isPitLimiterActive)
    {
        _isErsActive = false;
    }

    var throttle = _mainController.MoveIndicator.Z < 0;
    var speed = _mainController.GetShipSpeed();

    const float rechargeRate = 1f / 135;
    const float dischargeRate = 1f / 45;

    if (speed >= 1)
    {
        if (!_isErsActive || (_isErsActive && !throttle))
        {
            var factor = (float)MathHelper.Clamp(speed / DEFAULT_SUSPENSION_SPEED_LIMIT, 0f, 1f);
            _ersCharge += rechargeRate * factor * _delta;
        }
        else
        {
            var factor = 1;
            _ersCharge -= dischargeRate * factor * _delta;
        }
    }

    _ersCharge = MathHelper.Clamp(_ersCharge, 0, 1);

    if (_ersCharge <= 0)
    {
        _isErsActive = false;
    }

    foreach (var s in _suspensions)
    {
        s.Power = GetWheelPower();
    }

    var speedLimit = GetWheelSpeedLimit();
    SetSpeedLimit(speedLimit);

    var propulsion = _isErsActive && throttle ? ERS_PROPULSION_OVERRIDE : 0;
    SetPropulsionOverride(propulsion);

    foreach (var l in _ersLights)
    {
        l.Color = _isErsActive ? Color.Cyan : Color.Black;
    }
}

private void UpdateTyreDegradation()
{
    _currentTyres.Update(_mainController, _suspensions, _brakelights, _tyreLights, _data, _delta);

    SaveState();
}

private void UpdateAntenna()
{
    if (_antenna == null)
    {
        return;
    }

    _antenna.HudText = $"P{_data.Position}";
}

private void UpdateDraftingSensor()
{
    _draftingAuxList.Clear();

    if (_draftingSensor == null || _draftingSensor.Closed)
    {
        _isDrafting = false;
        return;
    }

    _draftingSensor.DetectedEntities(_draftingAuxList);

    var isBehindCar = _draftingAuxList.Any(x => !x.IsEmpty()
        && x.Type == MyDetectedEntityType.SmallGrid
        && !x.Name.Contains("Grid")
        && x.Velocity.Length() >= 70);

    var currentSpeed = _mainController.GetShipSpeed();

    if (isBehindCar && currentSpeed >= 50)
    {
        _draftingCooldown = DRAFTING_COOLDOWN;
    }

    if (_isPitLimiterActive)
    {
        _draftingCooldown = 0;
    }

    _isDrafting = _draftingCooldown > 0;

    foreach (var s in _suspensions)
    {
        s.Power = GetWheelPower();
        var speedLimit = GetWheelSpeedLimit();

        s.SetValueFloat("Speed Limit", speedLimit * 3.6f);
    }

    _draftingCooldown -= (int)(_delta * 1000);
}

private void SetupGridName()
{
    if (DRIVER_NUMBER <= 0 && DRIVER_NUMBER > 99)
    {
        throw new Exception("DRIVER_NUMBER should be between 1 and 99");
    }

    var teamTag = TEAM_TAG;

    if (TEAM_TAG == string.Empty)
    {
        teamTag = "XXX";
    }

    teamTag = teamTag.Trim()
        .Substring(0, 3)
        .ToUpper();

    Me.CubeGrid.CustomName = $"{teamTag} #{DRIVER_NUMBER:00}-{DRIVER_NAME.Trim()}";
}

private void SetupController()
{
    var controllerList = new List<IMyShipController>();
    GridTerminalSystem.GetBlocksOfType(controllerList);

    var control = controllerList.FirstOrDefault(c => c is IMyRemoteControl)
        ?? controllerList.FirstOrDefault(c => c is IMyCockpit);

    if (control == null)
    {
        throw new Exception("No cockpit or remote control!");
    }

    _mainController = control;
}

private void SetupSuspensions()
{
    var suspensions = new List<IMyMotorSuspension>();
    GridTerminalSystem.GetBlocksOfType(suspensions, s => s.CubeGrid == Me.CubeGrid);

    if (suspensions == null || suspensions.Count != 4)
    {
        throw new Exception("Need 4 suspensions!");
    }

    _suspensions = new IMyMotorSuspension[4];

    for (int i = 0; i < suspensions.Count; i++)
    {
        var suspension = suspensions[i];
        var worldDirection = suspension.GetPosition() - _mainController.CenterOfMass;
        var bodyPosition = Vector3D.TransformNormal(worldDirection, MatrixD.Transpose(_mainController.WorldMatrix));

        if (bodyPosition.X < 0)
        {
            if (bodyPosition.Z < 0)
            {
                suspension.CustomName = "Wheel Suspension FL";
                SetSuspension(SuspensionPosition.FrontLeft, suspension);
            }
            else if (bodyPosition.Z > 0)
            {
                suspension.CustomName = "Wheel Suspension RL";
                SetSuspension(SuspensionPosition.RearLeft, suspension);
            }
        }
        else if (bodyPosition.X > 0)
        {
            if (bodyPosition.Z < 0)
            {
                suspension.CustomName = "Wheel Suspension FR";
                SetSuspension(SuspensionPosition.FrontRight, suspension);
            }
            else if (bodyPosition.Z > 0)
            {
                suspension.CustomName = "Wheel Suspension RR";
                SetSuspension(SuspensionPosition.RearRight, suspension);
            }
        }
    }
}

private void SetupDisplays()
{
    _stringBuilder = new StringBuilder();
    _displays = new List<IMyTextSurface> { Me.GetSurface(0) };

    var display = (IMyTextSurface)GridTerminalSystem.GetBlockWithName(DISPLAY_NAME);

    if (display != null)
    {
        _displays.Add(display);
    }

    foreach (var d in _displays)
    {
        d.ContentType = ContentType.SCRIPT;
        d.Alignment = TextAlignment.CENTER;
        d.Script = string.Empty;
    }

    if (COCKPIT_DISPLAY_INDEX.HasValue)
    {
        var cockpit = _mainController as IMyCockpit;

        if (cockpit != null)
        {
            var d = cockpit.GetSurface(COCKPIT_DISPLAY_INDEX.GetValueOrDefault());

            if (d != null)
            {
                d.WriteText(string.Empty);
                d.ContentType = ContentType.TEXT_AND_IMAGE;
                d.Alignment = TextAlignment.CENTER;
                d.Font = "Monospace";

                _cockpitDisplay = d;
            }
        }
    }

    var textDisplay = (IMyTextSurface)GridTerminalSystem.GetBlockWithName(TEXT_DISPLAY_NAME);

    if (textDisplay != null)
    {
        textDisplay.WriteText(string.Empty);
        textDisplay.ContentType = ContentType.TEXT_AND_IMAGE;
        textDisplay.Alignment = TextAlignment.CENTER;
        textDisplay.Font = "Monospace";
        ((IMyTerminalBlock)textDisplay).CustomData = TEXT_DISPLAY_HUDLCD;

        _textDisplay = textDisplay;
    }

    var rankDisplay = (IMyTextSurface)GridTerminalSystem.GetBlockWithName(RANK_DISPLAY_NAME);

    if (rankDisplay != null)
    {
        rankDisplay.WriteText(string.Empty);
        rankDisplay.ContentType = ContentType.TEXT_AND_IMAGE;
        rankDisplay.Alignment = TextAlignment.CENTER;
        rankDisplay.Font = "Monospace";
        ((IMyTerminalBlock)rankDisplay).CustomData = RANK_DISPLAY_HUDLCD;

        _rankDisplay = rankDisplay;
    }
}

private void SetupBrakelights()
{
    var lights = new List<IMyLightingBlock>();

    GridTerminalSystem.GetBlockGroupWithName(BRAKELIGHT_GROUP_NAME)
        .GetBlocksOfType<IMyLightingBlock>(lights, b => b.CubeGrid == Me.CubeGrid);

    if (lights.Count <= 0)
    {
        throw new Exception($"\"{BRAKELIGHT_GROUP_NAME}\" group not set.");
    }

    _brakelights = new List<IMyLightingBlock>();

    foreach (var l in lights)
    {
        l.Intensity = 5f;
        l.BlinkLength = 50f;
        l.BlinkIntervalSeconds = 0f;

        _brakelights.Add(l);
    }

    _tyreLights = new List<IMyLightingBlock>();
    GridTerminalSystem.GetBlocksOfType(_tyreLights, b => b.CubeGrid != Me.CubeGrid);

    foreach (var l in _tyreLights)
    {
        l.BlinkLength = 50f;
        l.BlinkIntervalSeconds = 0;
    }
}

private void SetupDrsLights()
{
    _drsLights = new List<IMyLightingBlock>();
    var lights = new List<IMyTerminalBlock>();
    var group = GridTerminalSystem.GetBlockGroupWithName(DRS_LIGHTS_GROUP_NAME);

    if (group == null)
    {
        return;
    }

    group.GetBlocks(lights, b => b.CubeGrid == Me.CubeGrid);

    foreach (var l in lights)
    {
        var light = (IMyLightingBlock)l;
        _drsLights.Add(light);
    }
}

private void SetupErsLights()
{
    _ersLights = new List<IMyLightingBlock>();
    var lights = new List<IMyTerminalBlock>();
    var group = GridTerminalSystem.GetBlockGroupWithName(ERS_LIGHTS_GROUP_NAME);

    if (group == null)
    {
        return;
    }

    group.GetBlocks(lights, b => b.CubeGrid == Me.CubeGrid);

    foreach (var l in lights)
    {
        var light = (IMyLightingBlock)l;
        light.Radius = 4f;
        light.Intensity = 10f;
        light.BlinkLength = 50f;
        light.BlinkIntervalSeconds = 0.5f;

        _ersLights.Add(light);
    }
}

private void SetupDraftingSensor()
{
    var sensor = (IMySensorBlock)GridTerminalSystem.GetBlockWithName(DRAFTING_SENSOR_NAME);

    if (sensor == null)
    {
        return;
    }

    _draftingSensor = sensor;
    _draftingSensor.TopExtend = 50;
    _draftingSensor.BottomExtend = 0;
    _draftingSensor.RightExtend = 2.25f;
    _draftingSensor.LeftExtend = 2.25f;
    _draftingSensor.FrontExtend = 0;
    _draftingSensor.BackExtend = 2;
    _draftingSensor.DetectSmallShips = true;
    _draftingSensor.DetectLargeShips = false;
    _draftingSensor.DetectStations = false;
    _draftingSensor.DetectSubgrids = false;
    _draftingSensor.DetectAsteroids = false;
    _draftingSensor.DetectPlayers = false;

    _draftingAuxList = new List<MyDetectedEntityInfo>();
}

private void SetupGyros()
{
    var gyros = new List<IMyGyro>();

    GridTerminalSystem.GetBlocksOfType(gyros, x => x.CubeGrid == Me.CubeGrid);

    if (gyros.Count <= 0)
    {
        throw new Exception("No gyroscope found.");
    }

    _gyros = gyros;
}

private void LoadState()
{
    if (string.IsNullOrWhiteSpace(Me.CustomData))
    {
        SetTyres(TyreCompound.Soft);
        return;
    }

    var values = Me.CustomData.Split(';');

    if (values.Length < 3)
    {
        SetTyres(TyreCompound.Soft);
        return;
    }

    var compoundChar = Convert.ToChar(values[0]);
    var wearPercentage = (float)Convert.ToDouble(values[1]);
    var charge = (float)Convert.ToDouble(values[2]);

    _currentTyres = Tyre.Load(compoundChar, wearPercentage);
    _ersCharge = charge;
}

private void SetupAntenna()
{
    var antennas = new List<IMyRadioAntenna>();
    GridTerminalSystem.GetBlocksOfType(antennas);
    var antenna = antennas.FirstOrDefault();

    if (antenna == null)
    {
        throw new Exception("No antenna!");
    }

    antenna.Enabled = true;
    antenna.Radius = 5000;
    antenna.EnableBroadcasting = true;
    antenna.HudText = $"(P{_data.Position}) {DRIVER_NAME}-{DRIVER_NUMBER}";
    _antenna = antenna;
}

private void SetupBroadcastListener()
{
    IGC.RegisterBroadcastListener("Address");
    var listeners = new List<IMyBroadcastListener>();
    IGC.GetBroadcastListeners(listeners);
    _broadcastListener = listeners.FirstOrDefault();
}

private void HandleArgument(string argument)
{
    if (argument.Equals("LMT", StringComparison.InvariantCultureIgnoreCase))
    {
        _isPitLimiterActive = !_isPitLimiterActive;
        return;
    }

    if (argument.Equals("LMT_ON", StringComparison.InvariantCultureIgnoreCase))
    {
        _isPitLimiterActive = true;
        return;
    }

    if (argument.Equals("LMT_OFF", StringComparison.InvariantCultureIgnoreCase))
    {
        _isPitLimiterActive = false;
        return;
    }

    if (argument.Equals("DRS", StringComparison.InvariantCultureIgnoreCase))
    {
        _isDrsActive = !_isDrsActive;
        return;
    }

    if (argument.Equals("DRS_ON", StringComparison.InvariantCultureIgnoreCase))
    {
        _isDrsActive = true;
        return;
    }

    if (argument.Equals("DRS_OFF", StringComparison.InvariantCultureIgnoreCase))
    {
        _isDrsActive = false;
        return;
    }

    if (argument.Equals("ERS", StringComparison.InvariantCultureIgnoreCase))
    {
        _isErsActive = !_isErsActive;
        return;
    }

    if (argument.Equals("ERS_ON", StringComparison.InvariantCultureIgnoreCase))
    {
        _isErsActive = true;
        return;
    }

    if (argument.Equals("ERS_OFF", StringComparison.InvariantCultureIgnoreCase))
    {
        _isErsActive = false;
        return;
    }

    if (argument.Equals("ULTRA", StringComparison.InvariantCultureIgnoreCase))
    {
        ChangeTyres(TyreCompound.Ultra);
        return;
    }

    if (argument.Equals("SOFT", StringComparison.InvariantCultureIgnoreCase))
    {
        ChangeTyres(TyreCompound.Soft);
        return;
    }

    if (argument.Equals("MEDIUM", StringComparison.InvariantCultureIgnoreCase))
    {
        ChangeTyres(TyreCompound.Medium);
        return;
    }

    if (argument.Equals("HARD", StringComparison.InvariantCultureIgnoreCase))
    {
        ChangeTyres(TyreCompound.Hard);
        return;
    }

    if (argument.Equals("EXTRA", StringComparison.InvariantCultureIgnoreCase))
    {
        ChangeTyres(TyreCompound.Extra);
        return;
    }

    if (argument.Equals("INT", StringComparison.InvariantCultureIgnoreCase))
    {
        ChangeTyres(TyreCompound.Intermediate);
        return;
    }

    if (argument.Equals("WET", StringComparison.InvariantCultureIgnoreCase))
    {
        ChangeTyres(TyreCompound.Wet);
        return;
    }

    if (argument.Equals("FLIP", StringComparison.InvariantCultureIgnoreCase))
    {
        _doFlip = true;
        return;
    }

    if (argument.Equals("FLAG_G", StringComparison.InvariantCultureIgnoreCase))
    {
        RequestFlag(Flag.Green);
        return;
    }

    if (argument.Equals("FLAG_Y", StringComparison.InvariantCultureIgnoreCase))
    {
        RequestFlag(Flag.Yellow);
        return;
    }

    if (argument.Equals("FLAG_R", StringComparison.InvariantCultureIgnoreCase))
    {
        RequestFlag(Flag.Red);
        return;
    }
}

private void ChangeTyres(TyreCompound compound)
{
    if (!_isPitLimiterActive || _mainController.GetShipSpeed() > 1)
    {
        return;
    }

    SetTyres(compound);
    SaveState(true);
}

private void SetTyres(TyreCompound compound)
{
    switch (compound)
    {
        case TyreCompound.Ultra:
            _currentTyres = Tyre.NewUltras();
            break;

        case TyreCompound.Soft:
            _currentTyres = Tyre.NewSofts();
            break;

        case TyreCompound.Medium:
            _currentTyres = Tyre.NewMediums();
            break;

        case TyreCompound.Hard:
            _currentTyres = Tyre.NewHards();
            break;

        case TyreCompound.Extra:
            _currentTyres = Tyre.NewExtras();
            break;

        case TyreCompound.Intermediate:
            _currentTyres = Tyre.NewIntermediates();
            break;

        case TyreCompound.Wet:
            _currentTyres = Tyre.NewWets();
            break;

        default:
            break;
    }

    SetBrakelightColor(_currentTyres.Color);

    foreach (var s in _suspensions)
    {
        s.ApplyAction("Add Top Part");
        s.Friction = _currentTyres.MaxFriction;
    }
}

private void SetBrakelightColor(Color color)
{
    foreach (var l in _brakelights)
    {
        l.Color = color;
        l.BlinkIntervalSeconds = 0;
    }

    foreach (var l in _tyreLights)
    {
        if (l.IsSameConstructAs(Me))
        {
            l.Color = color;
            l.BlinkIntervalSeconds = 0;
        }
    }
}

private void SetSpeedLimit(float metersPerSecond)
{
    foreach (var s in _suspensions)
    {
        s.SetValueFloat("Speed Limit", metersPerSecond * 3.6f);
    }
}

private void SaveState(bool force = false)
{
    _saveStateCooldown -= (int)(_delta * 1000);

    if (!force && _saveStateCooldown > 0)
    {
        return;
    }

    var tyreChar = _currentTyres.Symbol;

    Me.CustomData = $"{tyreChar};{_currentTyres.WearPercentage};{_ersCharge}";
    _saveStateCooldown = SAVE_STATE_COOLDOWN;
}

private string GetCurrentFlagName()
{
    var flagName = string.Empty;

    switch (_data.CurrentFlag)
    {
        case Flag.Blue: flagName = "Blue"; break;
        case Flag.Green: flagName = "Green"; break;
        case Flag.Red: flagName = "Red"; break;
        case Flag.Yellow: flagName = "Yellow"; break;
    }

    return flagName;
}

private Color GetCurrentFlagColor()
{
    var color = Color.Black;

    switch (_data.CurrentFlag)
    {
        case Flag.Blue: color = Color.Blue; break;
        case Flag.Green: color = Color.Green; break;
        case Flag.Red: color = Color.Red; break;
        case Flag.Yellow: color = Color.Yellow; break;
    }

    return color;
}

private string BuildErsBar()
{
    const int barLength = 6;
    var arrow = _isErsActive
        ? ARROW_DOWN_CHAR
        : _ersCharge < 1
            ? ARROW_UP_CHAR
            : '-';

    var ersBar = arrow + "E:";

    for (int i = 0; i < barLength; i++)
    {
        var factor = 1f / barLength;

        if (_ersCharge > factor * i)
        {
            if (_ersCharge < factor * (i + 1))
            {
                ersBar += BLOCK_HALF_CHAR;
                continue;
            }

            ersBar += BLOCK_FILLED_CHAR;
        }
        else
        {
            ersBar += BLOCK_EMPTY_CHAR;
        }
    }

    return ersBar;
}

private string[] BuildVerticalBar(char title, int length, float currentValue, float maxValue)
{
    var strBar = new string[length + 2];
    strBar[0] = $"┌{title}┐";

    var perc = Math.Floor(100 * currentValue / maxValue);

    for (int i = 1; i < strBar.Length - 1; i++)
    {
        var mult = i - 1;
        var factor = 100f / length;
        var position = strBar.Length - 1 - i;

        if (perc > factor * mult)
        {
            if (perc < factor * (mult + 1))
            {
                strBar[position] = $"│{BLOCK_HALF_CHAR}│";

                continue;
            }

            strBar[position] = $"│{BLOCK_FILLED_CHAR}│";
        }
        else
        {
            strBar[position] = $"│{BLOCK_EMPTY_CHAR}│";
        }
    }

    strBar[strBar.Length - 1] = perc < 100 ? $"{perc + "%",3}" : $"{perc}";

    return strBar;
}

private void RequestFlag(Flag flag)
{
    if (_address <= 0)
    {
        return;
    }

    IGC.SendUnicastMessage(_address, "Flag", $"{(int)flag}");
}

private float GetWheelPower()
{
    if (_isPitLimiterActive)
    {
        return 20f;
    }

    if (_isDrafting || _isErsActive)
    {
        return 100f;
    }

    return DEFAULT_SUSPENSION_POWER;
}

private float GetWheelSpeedLimit()
{
    if (_isPitLimiterActive)
    {
        return 26;
    }

    if (_data.CurrentFlag == Flag.Yellow)
    {
        return 45;
    }

    if (_isDrafting)
    {
        return 999;
    }

    if (_isErsActive)
    {
        return 97f;
    }

    return DEFAULT_SUSPENSION_SPEED_LIMIT;
}

private IMyMotorSuspension GetSuspension(SuspensionPosition pos)
{
    return _suspensions[(int)pos];
}

private void SetSuspension(SuspensionPosition pos, IMyMotorSuspension suspension)
{
    if (suspension == null)
    {
        return;
    }

    _suspensions[(int)pos] = suspension;
}

private void SetPropulsionOverride(float value)
{
    var susFl = GetSuspension(SuspensionPosition.FrontLeft);
    var susFr = GetSuspension(SuspensionPosition.FrontRight);
    var susRl = GetSuspension(SuspensionPosition.RearLeft);
    var susRr = GetSuspension(SuspensionPosition.RearRight);

    susFl.PropulsionOverride = value;
    susFr.PropulsionOverride = -value;
    susRl.PropulsionOverride = value;
    susRr.PropulsionOverride = -value;
}

private float GetMirrorProximity(IMySensorBlock mirrorSensor)
{
    if (mirrorSensor == null || mirrorSensor.Closed || !mirrorSensor.IsActive)
    {
        return float.MaxValue;
    }

    _mirrorAuxList.Clear();
    mirrorSensor.DetectedEntities(_mirrorAuxList);

    if (_mirrorAuxList.Count <= 0)
    {
        return float.MaxValue;
    }

    var pos = Me.CubeGrid.GetPosition();
    var nearest = _mirrorAuxList.Select(x => Vector3.Distance(pos, x.Position))
        .Min();

    return nearest;
}

private string GetMirrorProximityArrows(bool isRightMirror)
{
    var prox = GetMirrorProximity(isRightMirror ? _mirrorRight : _mirrorLeft);

    if (prox == float.MaxValue)
    {
        return string.Empty;
    }

    var arrow = isRightMirror ? ARROW_RIGHT : ARROW_LEFT;

    if (prox < 15)
    {
        return $"{arrow}{arrow}{arrow}";
    }

    if (prox < 30)
    {
        return $"{arrow}{arrow}";
    }

    return $"{arrow}";
}

private char GetSectorStatusChar(LapSectorStatus sectorStatus)
{
    switch (sectorStatus)
    {
        case LapSectorStatus.NotSet: return _spinnerAnim.CurrentChar;
        case LapSectorStatus.Worse: return '-';
        case LapSectorStatus.Better: return '+';
        case LapSectorStatus.Best: return '*';
        default: return _spinnerAnim.CurrentChar;
    }
}

private Color GetSectorStatusColor(LapSectorStatus sectorStatus)
{
    switch (sectorStatus)
    {
        case LapSectorStatus.NotSet: return Color.White;
        case LapSectorStatus.Worse: return Color.Yellow;
        case LapSectorStatus.Better: return Color.Lime;
        case LapSectorStatus.Best: return Color.Magenta;
        default: return Color.White;
    }
}

private string CentralizeString(string text, int width)
{
    if (text.Length > width)
    {
        return text.Substring(0, width);
    }

    var spaceLeft = width - text.Length;
    var leftSide = (int)Math.Floor((float)spaceLeft / 2);
    var rightSide = (int)Math.Ceiling((float)spaceLeft / 2);

    var strBuilder = new StringBuilder(text);

    strBuilder.Insert(0, " ", leftSide);
    strBuilder.Append(' ', rightSide);

    return strBuilder.ToString();
}
private class CharacterAnimation
{
    private readonly char[] _frames;
    private int _cooldown;

    public int CurrentFrame { get; private set; }

    public char CurrentChar
    {
        get
        {
            return _frames[CurrentFrame];
        }
    }

    public int FrameCount { get; }

    public int FrameTimeMilliseconds { get; set; }

    public CharacterAnimation(char[] frames, int frameTime)
    {
        _frames = frames;
        FrameCount = frames.Length;
        FrameTimeMilliseconds = frameTime;
    }

    public void Update(float delta)
    {
        var deltaMillis = (int)(delta * 1000);
        _cooldown += deltaMillis;

        if (_cooldown >= FrameTimeMilliseconds)
        {
            CurrentFrame++;

            if (CurrentFrame == FrameCount)
            {
                CurrentFrame = 0;
            }

            _cooldown -= FrameTimeMilliseconds;
        }
    }

    public override string ToString()
    {
        return $"{CurrentChar}";
    }
}
private enum Flag
{
    Green,
    Yellow,
    Red,
    Blue
}
private class RaceData
{
    public int Position { get; set; }
    public int TotalRacers { get; set; }
    public int Laps { get; set; }
    public int TotalLaps { get; set; }
    public string CurrentLapTime { get; set; } = "--:--.---";
    public string BestLapTime { get; set; } = "--:--.---";
    public Flag CurrentFlag { get; set; }
    public WeatherLevel CurrentWeather { get; set; }
    public string RankTable { get; set; }
    public LapSectorStatus StatusS1 { get; set; }
    public LapSectorStatus StatusS2 { get; set; }
    public LapSectorStatus StatusS3 { get; set; }
    public string PrevLapTime { get; set; } = "--:--.---";

    public void Map(string data)
    {
        try
        {
            var values = data.Split(';');

            Laps = Convert.ToInt32(values[0]);
            Position = Convert.ToInt32(values[1]);
            CurrentLapTime = values[2];
            BestLapTime = values[3];
            TotalRacers = Convert.ToInt32(values[4]);
            TotalLaps = Convert.ToInt32(values[5]);
            CurrentFlag = (Flag)Convert.ToInt32(values[6]);
            CurrentWeather = (WeatherLevel)Convert.ToInt32(values[7]);
            RankTable = values[8];
            StatusS1 = (LapSectorStatus)Convert.ToInt32(values[9]);
            StatusS2 = (LapSectorStatus)Convert.ToInt32(values[10]);
            StatusS3 = (LapSectorStatus)Convert.ToInt32(values[11]);
            PrevLapTime = values[12];
        }
        catch (Exception) { }
    }
}
private enum LapSectorStatus
{
    NotSet,
    Worse,
    Better,
    Best
}
private enum SuspensionPosition
{
    FrontRight,
    FrontLeft,
    RearRight,
    RearLeft
}
private class Tyre
{
    private readonly float _wearFactor;

    public char Symbol { get; private set; }

    public float CurrentFriction { get { return CalculateCurrentFriction(); } }

    public float MaxFriction { get; private set; }

    public float MinFriction { get; private set; }

    public float WearPercentage { get; private set; }

    public int Lifespan { get; private set; }

    public bool IsSlick { get; private set; }

    public Color Color { get; private set; }

    private Tyre(int lifespan, float maxFriction, float minFriction, char symbol, Color color, bool isSlick = true)
    {
        Lifespan = lifespan;
        MaxFriction = maxFriction;
        MinFriction = minFriction;
        Symbol = symbol;
        Color = color;
        IsSlick = isSlick;
        WearPercentage = 1f;

        _wearFactor = 1f / (60 * Lifespan);
    }

    public void Update(IMyShipController mainController, IMyMotorSuspension[] suspensions,
        List<IMyLightingBlock> brakelights, List<IMyLightingBlock> tyreLights, RaceData data, float delta)
    {
        var speed = mainController.GetShipSpeed();

        if (speed < 1)
        {
            return;
        }

        var speedFactor = (float)MathHelper.Clamp(speed, 0, 90) / 90;
        var wearRate = _wearFactor * speedFactor * delta;

        WearPercentage -= wearRate * GetTyreWeariness(data.CurrentWeather);
        WearPercentage = MathHelper.Clamp(WearPercentage, 0, 1f);

        foreach (var s in suspensions)
        {
            s.Friction = CurrentFriction * GetTyreEfficiency(data.CurrentWeather);
        }

        if (WearPercentage <= 0.25f)
        {
            if (brakelights.Any(l => l.BlinkIntervalSeconds <= 0))
            {
                foreach (var l in brakelights)
                {
                    l.BlinkIntervalSeconds = 0.25f;
                }
            }

            if (tyreLights.Any(l => l.BlinkIntervalSeconds <= 0))
            {
                foreach (var l in tyreLights)
                {
                    l.BlinkIntervalSeconds = 0.25f;
                }
            }
        }
        else
        {
            if (brakelights.Any(l => l.BlinkIntervalSeconds > 0))
            {
                foreach (var l in brakelights)
                {
                    l.BlinkIntervalSeconds = 0f;
                }
            }

            if (tyreLights.Any(l => l.BlinkIntervalSeconds > 0))
            {
                foreach (var l in tyreLights)
                {
                    l.BlinkIntervalSeconds = 0f;
                }
            }
        }

        if (CurrentFriction <= MinFriction)
        {
            if (suspensions.All(s => s.IsAttached))
            {
                var rand = new Random().Next(4);
                suspensions[rand].Detach();
            }
        }
    }

    public static Tyre Load(char compoundSymbol, float wearPercentage)
    {
        Tyre result;

        switch (compoundSymbol)
        {
            case 'U': result = NewUltras(); break;
            case 'S': result = NewSofts(); break;
            case 'M': result = NewMediums(); break;
            case 'H': result = NewHards(); break;
            case 'X': result = NewExtras(); break;
            case 'I': result = NewIntermediates(); break;
            case 'W': result = NewWets(); break;
            default: result = NewSofts(); break;
        }

        result.WearPercentage = wearPercentage;

        return result;
    }

    public static Tyre NewUltras()
    {
        return new Tyre(5, 100, 80, 'U', new Color(192, 0, 255));
    }

    public static Tyre NewSofts()
    {
        return new Tyre(8, 100, 45, 'S', Color.Red);
    }

    public static Tyre NewMediums()
    {
        return new Tyre(13, 75, 45, 'M', Color.Yellow);
    }

    public static Tyre NewHards()
    {
        return new Tyre(21, 60, 45, 'H', Color.White);
    }

    public static Tyre NewExtras()
    {
        return new Tyre(34, 55, 45, 'X', new Color(255, 32, 0));
    }

    public static Tyre NewIntermediates()
    {
        return new Tyre(8, 60, 40, 'I', Color.Green, false);
    }

    public static Tyre NewWets()
    {
        return new Tyre(13, 50, 40, 'W', new Color(0, 16, 255), false);
    }

    private float GetTyreEfficiency(WeatherLevel weatherLevel)
    {
        switch (weatherLevel)
        {
            case WeatherLevel.Clear:
            case WeatherLevel.LightClouds:
            case WeatherLevel.Cloudy:
            case WeatherLevel.Overcast:
                return 1;
            case WeatherLevel.Drizzle:
                return IsSlick ? 0.75f : 1;
            case WeatherLevel.Rain:
                return IsSlick ? 0.5f : 1;
            case WeatherLevel.HeavyRain:
                return IsSlick ? 0.25f : (Symbol == 'I' ? 0.8f : 1);
            default:
                return 1;
        }
    }

    private float GetTyreWeariness(WeatherLevel weatherLevel)
    {
        switch (weatherLevel)
        {
            case WeatherLevel.Clear:
                return IsSlick ? 1 : (Symbol == 'W' ? 2f : 1.25f);
            case WeatherLevel.LightClouds:
                return Symbol == 'W' ? 1.8f : 1;
            case WeatherLevel.Cloudy:
                return Symbol == 'W' ? 1.6f : 1;
            case WeatherLevel.Overcast:
                return Symbol == 'W' ? 1.4f : 1;
            case WeatherLevel.Drizzle:
                return Symbol == 'W' ? 1.2f : 1;
            case WeatherLevel.Rain:
            case WeatherLevel.HeavyRain:
            default:
                return 1;
        }
    }

    private float CalculateCurrentFriction()
    {
        var rad = MathHelper.ToRadians(90 - WearPercentage * 90);
        return MaxFriction - ((MaxFriction - MinFriction) * (float)Math.Sin(rad));
    }
}
private enum TyreCompound
{
    Ultra,          //100% ~5 minutes
    Soft,           //100% ~8 minutes
    Medium,         //75%  ~13 minutes
    Hard,           //60%  ~21 minutes
    Extra,          //55%  ~34 minutes
    Intermediate,   //60%  ~8  minutes
    Wet             //50%  ~13 minutes
}
private class Weather
{
    private static Color ClearColor = new Color(255, 106, 0);
    private static Color LightCloudsColor = new Color(255, 233, 127);
    private static Color CloudyColor = new Color(255, 244, 191);
    private static Color OvercastColor = Color.White;
    private static Color DrizzleColor = new Color(127, 202, 255);
    private static Color RainColor = new Color(0, 148, 255);
    private static Color HeavyRainColor = Color.Blue;

    private const float WEATHER_UPDATE_TIME = 60000;
    private readonly WeatherLevel _initialLevel;
    private readonly int[][] _changeChances;
    private readonly Random _random;
    private float _time;

    public WeatherLevel Level { get; private set; }

    public bool Enabled { get; set; }

    public string Description
    {
        get { return GetWeatherDescription(Level); }
    }

    public Weather(WeatherLevel initialLevel = WeatherLevel.Clear)
    {
        _time = WEATHER_UPDATE_TIME;
        _changeChances = new int[][]
        {
            new int[] { 0,  60 },
            new int[] { 50, 30 },
            new int[] { 50, 20 },
            new int[] { 40, 40 },
            new int[] { 20, 50 },
            new int[] { 30, 50 },
            new int[] { 50,  0 },
        };

        _random = new Random();
        _initialLevel = initialLevel;
        Level = _initialLevel;
    }

    public void Update(float delta)
    {
        if (!Enabled)
        {
            Level = WeatherLevel.Clear;
            return;
        }

        _time -= (delta);

        if (_time <= 0)
        {
            ChangeLevel();
            _time += WEATHER_UPDATE_TIME;
        }
    }

    private void ChangeLevel()
    {
        var currentLevel = (int)Level;
        var chanceArray = _changeChances[currentLevel + 3];
        var downChance = chanceArray[0] / 10;
        var upChance = 11 - chanceArray[1] / 10;

        var roll = _random.Next(10) + 1;

        if (roll <= downChance)
        {
            currentLevel--;
        }
        else if (roll >= upChance)
        {
            currentLevel++;
        }

        Level = (WeatherLevel)MathHelper.Clamp(currentLevel, -3, 3);
    }

    public static string GetWeatherDescription(WeatherLevel weatherLevel)
    {
        switch (weatherLevel)
        {
            case WeatherLevel.Clear:
                return "Clear";

            case WeatherLevel.LightClouds:
                return "L. Clouds";

            case WeatherLevel.Cloudy:
                return "Cloudy";

            case WeatherLevel.Overcast:
                return "Overcast";

            case WeatherLevel.Drizzle:
                return "Drizzle";

            case WeatherLevel.Rain:
                return "Rain";

            case WeatherLevel.HeavyRain:
                return "H. Rain";

            default:
                return string.Empty;
        }
    }

    public static Color GetWeatherColor(WeatherLevel weatherLevel)
    {
        switch (weatherLevel)
        {
            case WeatherLevel.Clear:
                return ClearColor;

            case WeatherLevel.LightClouds:
                return LightCloudsColor;

            case WeatherLevel.Cloudy:
                return CloudyColor;

            case WeatherLevel.Overcast:
                return OvercastColor;

            case WeatherLevel.Drizzle:
                return DrizzleColor;

            case WeatherLevel.Rain:
                return RainColor;

            case WeatherLevel.HeavyRain:
                return HeavyRainColor;

            default:
                return Color.White;
        }
    }
}

private enum WeatherLevel
{
    Clear = -3,
    LightClouds = -2,
    Cloudy = -1,
    Overcast = 0,
    Drizzle = 1,
    Rain = 2,
    HeavyRain = 3
}
