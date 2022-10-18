#l "./devops/cake/infra.cake"
#l "./devops/cake/tests.cake"
#l "./devops/cake/sonar.cake"

var target = Argument("target", "RunTests");

RunTarget(target);