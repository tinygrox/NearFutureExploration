﻿// Utils
// ---------------------------------

using System;
using UnityEngine;
using System.Collections.Generic;

namespace NearFutureExploration
{
  internal static class Utils
  {

       
    // This function loads up some animationstates
    public static AnimationState[] SetUpAnimation(string animationName, Part part)
    {
      var states = new List<AnimationState>();
      foreach (var animation in part.FindModelAnimators(animationName))
      {
          var animationState = animation[animationName];
          animationState.speed = 0;
          animationState.enabled = true;
          // Clamp this or else weird things happen
          animationState.wrapMode = WrapMode.ClampForever;
          animation.Blend(animationName);
          states.Add(animationState);
      }
      // Convert 
      return states.ToArray();
    }
    public static string ToSI(double d, string format = null)
    {
      if (d == 0.0)
        return d.ToString(format);

      char[] incPrefixes = new[] { 'k', 'M', 'G', 'T', 'P', 'E', 'Z', 'Y' };
      char[] decPrefixes = new[] { 'm', '\u03bc', 'n', 'p', 'f', 'a', 'z', 'y' };

      int degree = Mathf.Clamp((int)Math.Floor(Math.Log10(Math.Abs(d)) / 3), -8, 8);
      if (degree == 0)
        return d.ToString(format) + " ";

      double scaled = d * Math.Pow(1000, -degree);

      char? prefix = null;

      switch (Math.Sign(degree))
      {
        case 1: prefix = incPrefixes[degree - 1]; break;
        case -1: prefix = decPrefixes[-degree - 1]; break;
      }

      return scaled.ToString(format) + " " + prefix;
    }
    // Returns true if ship is it atmoshpere
    public static bool VesselInAtmosphere(Vessel vessel)
        {
            if (vessel.atmDensity > 0d)
                return true;
            else
                return false;
        }


        // fix for deprecated Unity function
        public static void SetActiveRecursively(GameObject obj, bool active)
        {
            obj.SetActive(active);

            foreach (Transform child in obj.transform)
            {
                SetActiveRecursively(child.gameObject, active);
            }
        }

        // Converts to a time string from a seconds, accounting for kerbal time
        public static string FormatTimeString(double seconds)
        {
            double dayLength;
            double yearLength;
            double rem;
            if (GameSettings.KERBIN_TIME)
            {
                dayLength = 6d;
                yearLength = 426d;
            }
            else
            {
                dayLength = 24d;
                yearLength = 365d;
            }

            int years = (int)(seconds / (3600.0d * dayLength * yearLength));
            rem = seconds % (3600.0d * dayLength * yearLength);
            int days = (int)(rem / (3600.0d * dayLength));
            rem = rem % (3600.0d * dayLength);
            int hours = (int)(rem / (3600.0d));
            rem = rem % (3600.0d);
            int minutes = (int)(rem / (60.0d));
            int secs = (int)rem;

            string result = "";

            // draw years + days
            if (years > 0)
            {
                result += years.ToString() + "y ";
                result += days.ToString() + "d ";
                result += hours.ToString() + "h";
                result += minutes.ToString() + "m";
            }
            else if (days > 0)
            {
                result += days.ToString() + "d ";
                result += hours.ToString() + "h";
                result += minutes.ToString() + "m";
                result += secs.ToString() + "s";
            }
            else if (hours > 0)
            {
                result += hours.ToString() + "h ";
                result += minutes.ToString() + "m ";
                result += secs.ToString() + "s";
            }
            else if (minutes > 0)
            {
                result += minutes.ToString() + "m ";
                result += secs.ToString() + "s";
            }
            else if (seconds > 0)
            {
                result += secs.ToString() + "s";
            }
            else
            {
                result = "None";
            }


            return result;
        }

        
    }


    public enum RadiatorState
    {
        Deployed,
        Deploying,
        Retracted,
        Retracting,
        Broken,
    }
}
