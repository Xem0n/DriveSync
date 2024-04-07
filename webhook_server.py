import logging
import flask.cli
from flask import Flask, Response, request, g

app = Flask(__name__)

sync_manager = None
debug = False

@app.route("/notifications", methods=["POST"])
def notifications():
    global sync_manager, debug

    if debug:
        print(request.headers)

    if sync_manager:
        sync_manager.download()

    return Response(status=200)

def init(port, _sync_manager, _debug):
    global sync_manager, debug

    sync_manager = _sync_manager
    debug = _debug

    if not debug:
        log = logging.getLogger("werkzeug")
        log.setLevel(logging.ERROR)
        flask.cli.show_server_banner = lambda *args: None

    app.run(host="127.0.0.1" , port=port)
