import 'package:flutter/material.dart';
import 'package:share_it/utils/common/constants/common_constants.dart';
import 'package:share_it/utils/common/enums/push_type_enum.dart';
import 'package:share_it/utils/common/helpers/common_helpers.dart';
import 'package:share_it/utils/launch/constants/launch_constants.dart';

class Launch extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    RouteForwardHelper.push(context, RoutesConstants.home, seconds: TimerConstants.navigationPushSeconds, pushType: PushType.PushReplacement);

    return new Scaffold(
      body: new Container(
          decoration: BoxDecoration(
          image: DecorationImage(
            image: NetworkImage("https://i.pinimg.com/originals/90/80/60/9080607321ab98fa3e70dd24b2513a20.gif"),
            fit: BoxFit.cover,
          ),
        ),),
    );
  }
}
