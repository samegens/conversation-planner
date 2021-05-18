node('docker-image-builder') {
    def app

    stage('Clone repository') {
        checkout scm
    }

    stage('Build image') {
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