node('docker-image-builder') {
    def app

    stage('Clone repository') {
		checkout([
			$class: 'GitSCM',
			branches: scm.branches,
			extensions: scm.extensions + [[$class: 'CloneOption', depth: 0, noTags: false, reference: '', shallow: false]],
			userRemoteConfigs: scm.userRemoteConfigs
		])
   }

    stage('Build image') {
		// Remove all dangling images.
		sh 'docker image prune -f'
		
       app = docker.build("ongoonku/conversation-planner")
    }

    stage('Test image') {
        sh 'docker run --rm ongoonku/conversation-planner echo "hello world!"'
    }

    stage('Push image') {
		def version = sh(script: "git describe --tags --abbrev=0",
						 returnStdout: true
					  ).toString().trim();
		echo "Found version ${version}"
        docker.withRegistry('https://registry.hub.docker.com', 'docker-ongoonku') {
            app.push("${version}")
            app.push("latest")
        }
    }
}