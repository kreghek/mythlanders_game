using System;

namespace Client.GameScreens.Campaign.Ui;

internal static class CampaignStagesPanelHelper
{
    public static int CalcMin(int current, int count, int max)
    {
        var min = Math.Max(0, current - 1);

        var lowestValidIndex = count - max;

        if (current > lowestValidIndex)
        {
            return lowestValidIndex;
        }

        return min;
    }
}