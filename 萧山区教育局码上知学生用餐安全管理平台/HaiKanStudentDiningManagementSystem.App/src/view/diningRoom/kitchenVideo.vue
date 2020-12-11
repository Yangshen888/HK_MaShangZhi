<template>
  <div>
    <Card>
      <dz-table
        :totalCount="stores.kitchenVideo.query.totalCount"
        :pageSize="stores.kitchenVideo.query.pageSize"
        @on-page-change="handlePageChanged"
        @on-page-size-change="handlePageSizeChanged"
      >
        <div slot="searcher">
          <section class="dnc-toolbar-wrap">
            <Row :gutter="16">
              <Col span="16">
                <Form inline @submit.native.prevent>
                  <FormItem>
                    <Input
                      type="text"
                      search
                      :clearable="true"
                      v-model="stores.kitchenVideo.query.kw"
                      placeholder="输入视频名称搜索..."
                      @on-search="handleSearchKitchenVideo()"
                    />
                  </FormItem>
                  <FormItem>
                    <DatePicker
                      v-model="stores.kitchenVideo.query.kw2"
                      type="daterange"
                      placeholder="视频时间"
                      format="yyyy/MM/dd"
                      style="width: 200px"
                      @on-change="dateChage"
                    ></DatePicker>
                  </FormItem>
                </Form>
              </Col>
              <Col span="8" class="dnc-toolbar-btns">
                <ButtonGroup class="mr3">
                  <Button
                    v-can="'delete'"
                    class="txt-danger"
                    icon="md-trash"
                    title="删除"
                    @click="handleBatchCommand('delete')"
                  ></Button>
                  <!-- <Button
                    class="txt-success"
                    icon="md-redo"
                    title="恢复"
                    @click="handleBatchCommand('recover')"
                  ></Button>-->
                  <!--                  <Button-->
                  <!--                    class="txt-danger"-->
                  <!--                    icon="md-hand"-->
                  <!--                    title="禁用"-->
                  <!--                    @click="handleBatchCommand('forbidden')"-->
                  <!--                  ></Button>-->
                  <!--                  <Button-->
                  <!--                    class="txt-success"-->
                  <!--                    icon="md-checkmark"-->
                  <!--                    title="启用"-->
                  <!--                    @click="handleBatchCommand('normal')"-->
                  <!--                  ></Button>-->
                  <Button icon="md-refresh" title="刷新" @click="handleRefresh"></Button>
                </ButtonGroup>
                <Button
                  v-can="'create'"
                  icon="md-create"
                  type="primary"
                  @click="handleShowCreateWindow"
                  title="新增视频"
                >新增视频</Button>
              </Col>
            </Row>
          </section>
        </div>
        <Table
          slot="table"
          ref="tables"
          :border="false"
          size="small"
          :highlight-row="true"
          :data="stores.kitchenVideo.data"
          :columns="stores.kitchenVideo.columns"
          @on-select="handleSelect"
          @on-selection-change="handleSelectionChange"
          @on-refresh="handleRefresh"
          :row-class-name="rowClsRender"
          @on-page-change="handlePageChanged"
          @on-page-size-change="handlePageSizeChanged"
          @on-sort-change="handleSortChange"
        >
          <!-- <template slot-scope="{row,index}" slot="userType">
            <span>{{renderUserType(row.userType)}}</span>
          </template>-->
          <template slot-scope="{row,index}" slot="status">
            <Tag :color="renderStatus(row.state).color">{{renderStatus(row.state).text}}</Tag>
          </template>
          <template slot-scope="{ row, index }" slot="action">
            <Poptip confirm :transfer="true" title="确定要删除吗?" @on-ok="handleDelete(row)">
              <Tooltip placement="top" content="删除" :delay="1000" :transfer="true">
                <Button v-can="'delete'" type="error" size="small" shape="circle" icon="md-trash"></Button>
              </Tooltip>
            </Poptip>
            <Tooltip placement="top" content="编辑" :delay="1000" :transfer="true">
              <Button
                v-can="'edit'"
                type="primary"
                size="small"
                shape="circle"
                icon="md-create"
                @click="handleEdit(row)"
              ></Button>
            </Tooltip>
            <Tooltip placement="top" content="详情" :delay="1000" :transfer="true">
              <Button
                v-can="'show'"
                type="warning"
                size="small"
                shape="circle"
                icon="md-search"
                @click="handleShow(row)"
              ></Button>
            </Tooltip>
            <!--            <Tooltip placement="top" content="分配角色" :delay="1000" :transfer="true">-->
            <!--              <Button type="success" size="small" shape="circle" icon="md-contacts" @click="handleAssignRole(row)"></Button>-->
            <!--            </Tooltip>-->
          </template>
        </Table>
      </dz-table>
    </Card>
    <Drawer
      :title="formTitle"
      v-model="formModel.opened"
      width="600"
      :mask-closable="false"
      :mask="true"
      :styles="styles"
    >
      <Form
        :model="formModel.fields"
        ref="formKitchenVideo"
        :rules="formModel.rules"
        label-position="top"
      >
        <Row :gutter="16">
          <Col span="14">
            <FormItem label="视频名称" prop="name">
              <Input :readonly="checkShow()" v-model="formModel.fields.name" placeholder="请输入视频名称" />
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="14">
            <FormItem label="视频类型" prop="type">
              <Select v-model="formModel.fields.type" :disabled="checkShow()">
                <Option
                  v-for="item in stores.kitchenVideo.sources.types2"
                  :value="item.value"
                  :key="item.value"
                >{{item.text}}</Option>
              </Select>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="14">
            <FormItem label="日期和时间" prop="addTime">
              <Date-picker
                type="datetime"
                @on-change="formModel.fields.addTime=$event"
                :value="datetimeNow"
                format="yyyy-MM-dd HH:mm:ss"
                placeholder="请选择日期和时间"
                style="width: 400px"
              ></Date-picker>
            </FormItem>
          </Col>
        </Row>
        <Row v-show="!checkShow()">
          <Upload
            ref="upload"
            :action="actionurl"
            :headers="postheaders"
            :show-upload-list="true"
            :data="{filename:this.formModel.dFileName}"
            :on-success="showUpResult"
            :on-progress="toUpResult"
            :max-size="20480"
            :on-exceeded-size="handleMaxSize"
            style="float:left"
            :disabled="updisabled"
            :on-remove="deleteFile"
          >
            <Button type="primary" icon="ios-cloud-upload-outline" :loading="loadingStatus">上传视频</Button>
          </Upload>
        </Row>
        <Row v-if="checkShow()">
          <Card>
            <p slot="title">视频文件</p>
            <p>{{this.formModel.fields.accessory.split('\\')[1]}}</p>
            <!-- <Button @click="download">下载</Button> -->
          </Card>
        </Row>
      </Form>
      <div v-if="!checkShow()" class="demo-drawer-footer">
        <Button icon="md-checkmark-circle" type="primary" @click="handleSubmitKitchenVideo(0)">保 存</Button>
        <Button style="margin-left: 8px" icon="md-close" @click="formModel.opened = false">取 消</Button>
      </div>
    </Drawer>
  </div>
</template>

<script>
import DzTable from "_c/tables/dz-table.vue";
import {
  getKitchenVideoList,
  createKitchenVideo,
  loadKitchenVideo,
  editKitchenVideo,
  deleteKitchenVideo,
  batchCommand,
  deletetoFile
} from "@/api/diningRoom/kitchenVideo";
import { getStaffList } from "@/api/rbac/user";
import {
  globalvalidateIDCardNoMust,
  globalvalidatepwd,
  globalvalidateIsNotEmpty
} from "@/global/validate";
import config from "@/config";
import { getToken } from "@/libs/util";
export default {
  name: "kitchenVideo",
  components: {
    DzTable
  },
  data() {
    return {
      url: "",
      updisabled: false,
      loadingStatus: false,
      actionurl: "",
      datetimeNow: "",
      postheaders: "",
      commands: {
        delete: { name: "delete", title: "删除" },
        recover: { name: "recover", title: "恢复" }
        // forbidden: { name: "forbidden", title: "禁用" },
        // normal: { name: "normal", title: "启用" }
      },
      formModel: {
        opened: false,
        title: "创建用户",
        mode: "create",
        dFileName: "xxxx",
        selection: [],
        selectPeople: [],
        fields: {
          videoUuid: "",
          name: "",
          addTime: "",
          accessory: "",
          type: "",
          addPeople: "",
          isDelete: "",
          schoolUuid: "",
          schoolName: ""
        },
        rules: {
          name: [
            {
              validator: globalvalidateIsNotEmpty,
              type: "string",
              required: true,
              message: "请输入视频名称"
            }
          ],
          addTime: [
            {
              required: true,
              message: "选择时间",
            }
          ],
          type: [{ required: true, message: "请选择类型" }],
          selectPeople: [{ required: true, message: "请选择人员" }]
        }
      },
      formAssignRole: {
        userGuid: "",
        opened: false,
        ownedRoles: [],
        inited: false,
        roles: []
      },
      stores: {
        staffList: [],
        kitchenVideo: {
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            kw: "",
            kw2: "",
            isDeleted: 0,
            status: -1,
            sort: [
              {
                direct: "DESC",
                field: "ID"
              }
            ]
          },
          sources: {
            types2: [
              { value: "厨房", text: "厨房" },
              { value: "餐厅", text: "餐厅" }
            ],
            types1: [
              { value: "荤", text: "荤" },
              { value: "素", text: "素" },
              { value: "作料", text: "作料" },
              { value: "其他", text: "其他" }
            ]
          },
          columns: [
            { type: "selection", width: 50, key: "handle" },
            { title: "视频名称", key: "name", sortable: true },
            { title: "类型", key: "type" },
            { title: "上传时间", key: "addTime" },
            {
              title: "操作",
              align: "center",
              width: 150,
              className: "table-command-column",
              slot: "action"
            }
          ],
          data: []
        }
      },
      styles: {
        height: "calc(100% - 55px)",
        overflow: "auto",
        paddingBottom: "53px",
        position: "static"
      },
      rolelist: [],
      departmentlist: [],
      schoolList: []
    };
  },
  computed: {
    formTitle() {
      if (this.formModel.mode === "create") {
        return "创建视频上传记录";
      }
      if (this.formModel.mode === "edit") {
        return "编辑视频上传记录";
      }
      if (this.formModel.mode === "show") {
        let s = "视频上传详情";
        if (
          this.formModel.fields.schoolName != "" &&
          this.$store.state.user.schoolguid == null
        ) {
          s += "   学校:(" + this.formModel.fields.schoolName + ")";
        }
        return s;
        // return "采购记录详情";
      }
      return "";
    },
    selectedRows() {
      return this.formModel.selection;
    },
    selectedRowsId() {
      return this.formModel.selection.map(x => x.videoUuid);
    }
  },
  methods: {
    loadKitchenVideoList() {
      console.log(this.stores.kitchenVideo.query.kw2);
      getKitchenVideoList(this.stores.kitchenVideo.query).then(res => {
        console.log(res.data.data);
        this.stores.kitchenVideo.data = res.data.data;
        this.stores.kitchenVideo.query.totalCount = res.data.totalCount;
      });
    },

    handleOpenFormWindow() {
      this.formModel.opened = true;
    },
    handleCloseFormWindow() {
      this.formModel.opened = false;
    },
    handleSwitchFormModeToCreate() {
      this.formModel.mode = "create";
    },
    handleSwitchFormModeToEdit() {
      this.formModel.mode = "edit";
      this.handleOpenFormWindow();
    },
    handleSwitchFormModeToShow() {
      this.formModel.mode = "show";
      this.handleOpenFormWindow();
    },
    handleEdit(row) {
      // this.defaultfilelist = [];
      this.formModel.dFileName = "";
      this.$refs.upload.fileList = [
        { name: "", status: "finished", showProgress: false }
      ];
      this.loadStaffList();
      this.handleSwitchFormModeToEdit();
      this.handleResetFormKitchenVideo();
      this.doLoadKitchenVideo(row.videoUuid);
    },
    handleShow(row) {
      this.loadStaffList();
      this.handleSwitchFormModeToShow();
      this.handleResetFormKitchenVideo();
      this.doLoadKitchenVideo(row.videoUuid);
    },
    handleSelect(selection, row) {},
    handleSelectionChange(selection) {
      this.formModel.selection = selection;
    },
    handleRefresh() {
      this.loadKitchenVideoList();
    },
    handleShowCreateWindow() {
      this.formModel.dFileName = "";
      this.$refs.upload.clearFiles();
      this.loadStaffList();
      this.handleSwitchFormModeToCreate();
      this.handleOpenFormWindow();
      this.handleResetFormKitchenVideo();
      let datetime = new Date();
      this.datetimeNow =
        datetime.getFullYear() +
        "-" +
        (datetime.getMonth() + 1) +
        "-" +
        datetime.getDate() +
        " " +
        datetime.getHours() +
        ":" +
        datetime.getMinutes() + 
        ":" +
        datetime.getSeconds();
      this.formModel.fields.addTime = this.datetimeNow;
    },
    handleSubmitKitchenVideo(num) {
      console.log(this.formModel.fields.accessory);
      if (
        this.formModel.fields.accessory == null ||
        this.formModel.fields.accessory == ""
      ) {
        this.$Message.warning("请上传视频");
        return;
      }
      let valid = this.validateKitchenVideoForm();
      //console.log(valid);
      console.log(this.formModel.fields);
      if (valid) {
        if (this.formModel.mode === "create") {
          this.doCreateKitchenVideo(num);
        }
        if (this.formModel.mode === "edit") {
          this.doEditKitchenVideo(num);
        }
      }
    },
    handleResetFormKitchenVideo() {
      this.$refs.formKitchenVideo.resetFields();
      // this.$refs["formKitchenVideo2"].resetFields();
      this.formModel.fields.accessory = "";
      this.formModel.fields.videoUuid = "";
      this.formModel.fields.schoolUuid = "";
    },
    doCreateKitchenVideo(num) {
      console.log(this.$store.state.user);
      this.formModel.fields.state = num.toString();
      this.formModel.fields.addPeople = this.$store.state.user.userName;
      this.formModel.fields.schoolUuid = this.$store.state.user.schoolguid;
      createKitchenVideo(this.formModel.fields).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.handleCloseFormWindow();
          this.loadKitchenVideoList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    doEditKitchenVideo(num) {
      this.formModel.fields.state = num.toString();
      this.formModel.fields.addPeople = this.$store.state.user.userName;
      editKitchenVideo(this.formModel.fields).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.handleCloseFormWindow();
          this.loadKitchenVideoList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    validateKitchenVideoForm() {
      let _valid = false;
      this.$refs["formKitchenVideo"].validate(valid => {
        if (!valid) {
          this.$Message.error("请完善表单信息");
          return _valid;
        } else {
          _valid = true;
        }
      });
      return _valid;
    },
    doLoadKitchenVideo(uuid) {
      loadKitchenVideo({ guid: uuid }).then(res => {
        console.log(res.data.data);
        this.formModel.fields = res.data.data.entity;
        console.log(1111);
        this.$refs.upload.fileList[0].name = this.formModel.fields.accessory.split(
          "\\"
        )[1];
        this.formModel.dFileName = res.data.data.path;
        this.url =
          config.baseUrl.dev +
          this.formModel.fields.accessory.replace("\\", "/");
        //this.formModel.selectPeople = res.data.data.systemUserUuid.split(",");
        // console.log(this.defaultfilelist);
      });
    },
    handleDelete(row) {
      this.doDelete(row.videoUuid);
    },
    doDelete(ids) {
      if (!ids) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      deleteKitchenVideo(ids).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadKitchenVideoList();
          this.formModel.selection = [];
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    handleBatchCommand(command) {
      if (!this.selectedRowsId || this.selectedRowsId.length <= 0) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      this.$Modal.confirm({
        title: "操作提示",
        content:
          "<p>确定要执行当前 [" +
          this.commands[command].title +
          "] 操作吗?</p>",
        loading: true,
        onOk: () => {
          this.doBatchCommand(command);
        }
      });
    },
    doBatchCommand(command) {
      batchCommand({
        command: command,
        ids: this.selectedRowsId.join(",")
      }).then(res => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadKitchenVideoList();
          this.formModel.selection = [];
        } else {
          this.$Message.warning(res.data.message);
        }
        this.$Modal.remove();
      });
    },
    handleSearchKitchenVideo() {
      console.log(this.stores.kitchenVideo.query.kw2);
      this.loadKitchenVideoList();
    },
    rowClsRender(row, index) {
      if (row.isDeleted) {
        return "table-row-disabled";
      }
      return "";
    },
    handleSortChange(column) {
      this.stores.kitchenVideo.query.sort.direction = column.order;
      this.stores.kitchenVideo.query.sort.field = column.key;
      this.loadKitchenVideoList();
    },
    handlePageChanged(page) {
      this.stores.kitchenVideo.query.currentPage = page;
      this.loadKitchenVideoList();
    },
    handlePageSizeChanged(pageSize) {
      this.stores.kitchenVideo.query.pageSize = pageSize;
      this.loadKitchenVideoList();
    },
    renderUserType(userType) {
      // console.log(userType);
      var userTypeText = "未知";
      if (userType != null && userType != "") {
        userTypeText = userType;
      }
      // switch (userType) {
      //   case 0:
      //     userTypeText = "超级管理员";
      //     break;
      //   case 1:
      //     userTypeText = "管理员";
      //     break;
      //   case 2:
      //     userTypeText = "普通用户";
      //     break;
      // }
      return userTypeText;
    },
    renderStatus(status) {
      // console.log(status);
      let _status = {
        color: "success",
        text: "已提交"
      };
      switch (status) {
        case "0":
          _status.text = "保存中";
          _status.color = "primary";
          break;
      }
      return _status;
    },
    changePeople(e) {
      console.log(e);
      this.formModel.fields.systemUserUuid = e.join();
    },
    loadStaffList() {
      getStaffList().then(res => {
        console.log(res);
        this.stores.staffList = res.data.data;
      });
    },
    dateChage(e) {
      console.log(e);
      if (e != null && e.length == 2) {
        this.handleSearchKitchenVideo();
      }
    },
    checkShow() {
      return this.formModel.mode === "show";
    },
    showUpResult(e) {
      this.loadingStatus = false;
      this.updisabled = false;
      console.log(e);
      if (e.code == "200") {
        this.$Message.success(e.message);
        this.formModel.fields.accessory = e.data.dataPath;
        this.formModel.dFileName = e.data.path;
        console.log(
          (this.$refs.upload.fileList[0].name = e.data.dataPath.split("\\")[1])
        );
        // console.log(this.defaultfilelist);
        // if (this.departmentlist.length >= 1) {
        //   this.defaultfilelist = [
        //     { name: this.formModel.fields.name, url: e.data.path }
        //   ];
        // }
        console.log(this.formModel.dFileName);
      } else {
        this.$Message.warning(e.message);
      }
    },
    toUpResult() {
      console.log(this.$refs.upload.fileList);
      //console.log(this.$refs.upload.fileList);
      if (this.$refs.upload.fileList.length > 1) {
        this.$refs.upload.fileList.shift();
        // this.$refs.upload.clearFiles();
        // this.$refs.upload.push({})
      }
      this.loadingStatus = true;
      this.updisabled = true;
    },
    deleteFile(e) {
      console.log(e);
      console.log(this.formModel.dFileName);
      // if (this.formModel.dFileName != null && this.formModel.dFileName != "") {
      //   deletetoFile({ filename: this.formModel.dFileName }).then(res => {
      //     console.log(res);
      //   });
      // }
    },
    download() {
      // console.log(this.url);
      window.location.href =
        config.baseUrl.dev + this.formModel.fields.accessory.replace("\\", "/");
    },
    handleMaxSize(file) {
      this.$Notice.warning({
        title: "文件过大",
        desc: file.name + "超过20M"
      });
    }
  },
  mounted() {
    console.log(this.$store.state.user);
    this.loadKitchenVideoList();
    console.log(this.$refs.upload.fileList);
  },
  created() {
    if (this.$store.state.user.schoolguid == null) {
      this.stores.kitchenVideo.columns.splice(1, 0, {
        title: "学校",
        key: "schoolName"
      });
    }
    this.postheaders = {
      Authorization: "Bearer " + getToken()
      // Origin: "http://localhost:3000",

      //Accept: "application/json, text/plain, */*"
    };
    this.actionurl =
      config.baseUrl.dev + "api/v1/DiningRoom/KitchenVideo/UpLoad/";
  }
};
</script>

<style>
</style>
