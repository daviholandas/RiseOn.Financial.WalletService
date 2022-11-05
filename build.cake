#l "./devops/cake/infra.cake"
#l "./devops/cake/tests.cake"
#l "./devops/cake/sonar.cake"
#l "./devops/cake/service.cake"

var target = Argument("target", "RunTests");

RunTarget(target);