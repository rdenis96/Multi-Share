import 'dart:async';

import 'package:flutter/material.dart';
import 'package:share_it/utils/common/enums/common_enums.dart';

class RouteForwardHelper {
  static void push(BuildContext context, String route,
      {int seconds = 0, PushType pushType = PushType.Push}) {

    switch (pushType) {
      case PushType.Push:
        Timer(Duration(seconds: seconds),
            () => Navigator.pushNamed(context, route));
        break;
      case PushType.PopAndPush:
        Timer(Duration(seconds: seconds),
            () => Navigator.popAndPushNamed(context, route));
        break;
      case PushType.PushReplacement:
        Timer(Duration(seconds: seconds),
            () => Navigator.pushReplacementNamed(context, route));
        break;
    }
  }
}
