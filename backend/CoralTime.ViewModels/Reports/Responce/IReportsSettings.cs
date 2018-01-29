﻿using System;

namespace CoralTime.ViewModels.Reports.Responce
{
    public interface IReportsSettings
    {
        DateTime? DateFrom { get; set; }

        DateTime? DateTo { get; set; }

        int? GroupById { get; set; }

        string FilterClientIds { get; set; }

        string FilterProjectIds { get; set; }

        string FilterMemberIds { get; set; }

        string FilterShowColumnIds { get; set; }
    }
}
