﻿using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace devoctomy.Passchamp.Core.Graph.Services;

public interface IInputPinsJsonParserService
{
    Dictionary<string, IPin> Parse(JArray json);
}
