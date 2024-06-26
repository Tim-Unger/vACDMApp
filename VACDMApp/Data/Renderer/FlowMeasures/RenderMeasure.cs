﻿using Java.Nio.FileNio;
using System.Text;
using System.Text.RegularExpressions;

namespace VacdmApp.Data.Renderer
{
    internal partial class FlowMeasures
    {
        private static readonly Color _darkBlue = new(28, 40, 54);

        private static readonly GridLength _oneStar = new(1, GridUnitType.Star);

        private static Grid RenderMeasure(FlowMeasure measure)
        {
            var grid = new Grid() { Background = _darkBlue, Margin = 10 };

            var status = GetStatus(measure);

            grid.ColumnDefinitions.Add(new ColumnDefinition(_oneStar));
            grid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(40, GridUnitType.Star)));

            var contentGrid = new Grid();
            contentGrid.RowDefinitions.Add(new RowDefinition(_oneStar));
            contentGrid.RowDefinitions.Add(new RowDefinition(_oneStar));
            contentGrid.RowDefinitions.Add(new RowDefinition(_oneStar));
            contentGrid.RowDefinitions.Add(new RowDefinition(_oneStar));
            contentGrid.RowDefinitions.Add(new RowDefinition(_oneStar));

            var nameGrid = new Grid();

            var identRegex = new Regex("([A-Z]{3,4})([0-9]{1,4})([A-Z])?");

            var ident = measure.Ident;

            var identString = identRegex.IsMatch(ident) ? GetIdentString(identRegex.Match(ident).Groups) : ident;

            var nameLabel = new Label()
            {
                Text = identString,
                TextColor = Colors.White,
                FontAttributes = FontAttributes.Bold,
                FontSize = 20,
                Margin = 5,
                HorizontalOptions = LayoutOptions.Start,
            };

            var statusLabel = new Label()
            {
                Text = measure.MeasureStatus.ToString(),
                TextColor = Colors.White,
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                Margin = 5,
                HorizontalOptions = LayoutOptions.End
            };

            nameGrid.Children.Add(nameLabel);
            nameGrid.Children.Add(statusLabel);

            contentGrid.Children.Add(nameGrid);
            contentGrid.SetRow(nameGrid, 0);

            if (measure.IsWithdrawn)
            {
                grid.Children.Add(contentGrid);
                grid.SetColumn(contentGrid, 1);

                var withdrawnColorGrid = new Grid() { Background = status.Color };
                grid.Children.Add(withdrawnColorGrid);
                grid.SetColumn(withdrawnColorGrid, 0);

                return grid;
            }

            var timeSpanLabel = new Label()
            {
                Text = $"{measure.StartTime:dd.MM. HH:mmZ} - {GetEndTimeString(measure.EndTime, measure.StartTime.Day)}",
                TextColor = Colors.White,
                FontAttributes = FontAttributes.None,
                FontSize = 20,
                Margin = 5,
                HorizontalOptions = LayoutOptions.Start
            };

            contentGrid.Children.Add(timeSpanLabel);
            contentGrid.SetRow(timeSpanLabel, 1);

            var flowMeasure = measure.Measure;

            var measureValueString = ParseValue(flowMeasure.Value);

            var measureType = flowMeasure.MeasureType;

            if (measureType == MeasureType.MDI || measureType == MeasureType.ADI)
            {
                var measureValueRaw = flowMeasure.Value ?? throw new InvalidDataException();

                var measureValue = int.Parse(measureValueRaw.ToString());

                measureValueString = GetTimeString(measureValue);
            }

            var typeLabel = new Label()
            {
                Text = $"{flowMeasure.MeasureTypeString}: {measureValueString}",
                TextColor = Colors.White,
                FontSize = 20,
                Margin = 5,
            };

            if(measureType == MeasureType.GroundStop)
            {
                typeLabel.Text = "Ground Stop";
            }

            contentGrid.Children.Add(typeLabel);
            contentGrid.SetRow(typeLabel, 2);

            var depAirportsFilter = measure.Filters.FirstOrDefault(x => x.Type == "ADEP");

            var depAirportsString = "";
            if(depAirportsFilter is not null)
            {
                var depValuesRaw = depAirportsFilter.Value.ToString();

                try
                {
                    var depValues = JsonSerializer.Deserialize<List<string>>(depValuesRaw);

                    depAirportsString = ConcatAirports(depValues.Select(x => x.ToString()).ToArray(), true);
                }
                catch
                {
                    depAirportsString = ConcatAirports(new[] { depValuesRaw }, true);
                }
            }

            var depAirportsLabel = new Label()
            {
                Text = depAirportsString,
                TextColor = Colors.White,
                FontSize = 20,
                Margin = 5,
            };

            contentGrid.Children.Add(depAirportsLabel);
            contentGrid.SetRow(depAirportsLabel, 3);

            var arrAirportsFilter = measure.Filters.FirstOrDefault(x => x.Type == "ADES");

            var arrAirportsString = "";
            if(arrAirportsFilter is not null)
            {
                var arrValuesRaw = arrAirportsFilter.Value.ToString();

                try
                {
                    var arrValues = JsonSerializer.Deserialize<List<string>>(arrValuesRaw);
                    arrAirportsString = ConcatAirports(arrValues.Select(x => x.ToString()).ToArray(), false);
                }
                catch
                {
                    arrAirportsString = ConcatAirports(new[] { arrValuesRaw }, false);
                }
            }

            var arrAirportsLabel = new Label()
            {
                Text = arrAirportsString,
                TextColor = Colors.White,
                FontSize = 20,
                Margin = 5,
            };

            contentGrid.Children.Add(arrAirportsLabel);
            contentGrid.SetRow(arrAirportsLabel, 4);

            grid.Children.Add(contentGrid);
            grid.SetColumn(contentGrid, 1);

            var statusColorGrid = new Grid() { Background = status.Color };
            grid.Children.Add(statusColorGrid);
            grid.SetColumn(statusColorGrid, 0);

            return grid;
        }

        private static string GetTimeString(int seconds) => seconds > 60 ? $"{seconds / 60} min." : $"{seconds} sec.";

        private static string ConcatAirports(string[] airports, bool isDep)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append(isDep ? "DEP: " : "ARR: ");

            foreach (var airport in airports.Take(airports.Length - 1))
            {
                stringBuilder.Append($"{airport}, ");
            }

            stringBuilder.Append(airports.Last());

            return stringBuilder.ToString();
        }

        private static string GetEndTimeString(DateTime endDateTime, int startDate) => endDateTime.Day == startDate ? endDateTime.ToString("HH:mmZ") : endDateTime.ToString("dd. HH:mmZ");

        private static string GetIdentString(GroupCollection groups) => $"{groups[1].Value}{groups[2].Value} - {groups[3].Value}";

        private static string ParseValue(object? rawValue)
        {
            if(rawValue is null)
            {
                return "";
            }

            //Check if the value is a Json Array
            try
            {
                var parsedValue = JsonSerializer.Deserialize<string[]>(rawValue.ToString());

                return string.Join(" ", parsedValue);
            }
            catch
            {
                return rawValue.ToString();
            }
        }
    }
}
