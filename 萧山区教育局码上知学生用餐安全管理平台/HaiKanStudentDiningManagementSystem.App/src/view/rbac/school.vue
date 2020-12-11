<template>
  <div>
    <Card>
      <dz-table
        :totalCount="stores.school.query.totalCount"
        :pageSize="stores.school.query.pageSize"
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
                      v-model="stores.school.query.kw"
                      placeholder="输入关键字搜索..."
                      @on-search="handleSearchSchool()"
                    >
                      <Select
                        slot="prepend"
                        v-model="stores.school.query.isDeleted"
                        @on-change="handleSearchSchool"
                        placeholder="删除状态"
                        style="width: 60px"
                      >
                        <Option
                          v-for="item in stores.school.sources.isDeletedSources"
                          :value="item.value"
                          :key="item.value"
                          >{{ item.text }}</Option
                        >
                      </Select>
                      <!--                      <Select-->
                      <!--                        slot="prepend"-->
                      <!--                        v-model="stores.user.query.status"-->
                      <!--                        @on-change="handleSearchUser"-->
                      <!--                        placeholder="用户状态"-->
                      <!--                        style="width:60px;"-->
                      <!--                      >-->
                      <!--                        <Option-->
                      <!--                          v-for="item in stores.user.sources.statusSources"-->
                      <!--                          :value="item.value"-->
                      <!--                          :key="item.value"-->
                      <!--                        >{{item.text}}</Option>-->
                      <!--                      </Select>-->
                    </Input>
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
                  <Button
                    v-can="'recover'"
                    class="txt-success"
                    icon="md-redo"
                    title="恢复"
                    @click="handleBatchCommand('recover')"
                  ></Button>
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
                  <Button
                    icon="md-refresh"
                    title="刷新"
                    @click="handleRefresh"
                  ></Button>
                </ButtonGroup>
                <!-- <Button type="primary" :loading="loading" @click="toSync">同步食材</Button> -->
                <Button
                  v-can="'create'"
                  icon="md-create"
                  type="primary"
                  @click="handleShowCreateWindow"
                  title="新增学校"
                  >新增学校</Button
                >
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
          :data="stores.school.data"
          :columns="stores.school.columns"
          @on-select="handleSelect"
          @on-selection-change="handleSelectionChange"
          @on-refresh="handleRefresh"
          :row-class-name="rowClsRender"
          @on-page-change="handlePageChanged"
          @on-page-size-change="handlePageSizeChanged"
          @on-sort-change="handleSortChange"
        >
          <!-- <template slot-scope="{row,index}" slot="userType">
            <span>{{renderSchoolType(row.userType)}}</span>
          </template>
          <template slot-scope="{row,index}" slot="status">
            <Tag :color="renderStatus(row.status).color">{{renderStatus(row.status).text}}</Tag>
          </template>-->
          <template slot-scope="{ row, index }" slot="action">
            <Poptip
              v-if="row.isDelete == 0"
              confirm
              :transfer="true"
              title="确定要删除吗?"
              @on-ok="handleDelete(row)"
            >
              <Tooltip
                placement="top"
                content="删除"
                :delay="1000"
                :transfer="true"
              >
                <Button
                  v-can="'delete'"
                  type="error"
                  size="small"
                  shape="circle"
                  icon="md-trash"
                ></Button>
              </Tooltip>
            </Poptip>
            <Tooltip
              placement="top"
              content="编辑"
              :delay="1000"
              :transfer="true"
            >
              <Button
                v-can="'edit'"
                type="primary"
                size="small"
                shape="circle"
                icon="md-create"
                @click="handleEdit(row)"
              ></Button>
            </Tooltip>
            <Tooltip
              placement="top"
              content="查看"
              :delay="1000"
              :transfer="true"
            >
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
    >
      <Form
        :model="formModel.fields"
        ref="formSchool"
        :rules="formModel.rules"
        label-position="top"
      >
        <Row :gutter="16">
          <Col span="14">
            <FormItem label="学校名称" prop="schoolName">
              <Input
                :readonly="formModel.mode == 'show'"
                v-model="formModel.fields.schoolName"
                placeholder="请输入学校名称"
              />
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="14">
            <FormItem label="学校类型" prop="cuisinetype">
              <Select v-model="formModel.schoolType" @on-change="changetype">
                <Option
                  v-for="item in formModel.cuisinetype"
                  :value="item.type"
                  :key="item.type"
                  >{{ item.type }}</Option
                >
              </Select>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <FormItem label="采购标准" prop="purchasingStandard">
            <Input
              :readonly="formModel.mode == 'show'"
              type="textarea"
              :autosize="{ minRows: 5, maxRows: 20 }"
              v-model="formModel.fields.purchasingStandard"
              placeholder="请输入采购标准"
            />
          </FormItem>
        </Row>
        <Row :gutter="16">
          <Col span="14">
            <FormItem label="学校饭卡充值链接" prop="link">
              <Input
                :readonly="formModel.mode == 'show'"
                v-model="formModel.fields.link"
                placeholder="请输入链接"
              />
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="14">
            <FormItem label="是否显示菜价" prop="isCuiPrices">
              <i-switch
                v-model="formModel.fields.isCuiPrices"
                :disabled="formModel.mode == 'show'"
              >
                <span slot="open">是</span>
                <span slot="close">否</span>
              </i-switch>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16" v-if="isshowptjob">
          <Col span="14">
            <FormItem label="是否启用勤工俭学" prop="isptjob">
              <i-switch
                v-model="formModel.fields.isptjob"
                :disabled="formModel.mode == 'show'"
              >
                <span slot="open">是</span>
                <span slot="close">否</span>
              </i-switch>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="14">
            <FormItem label="是否启用饭卡充值和在线订餐" prop="isrecharge">
              <i-switch
                v-model="formModel.fields.isrecharge"
                :disabled="formModel.mode == 'show'"
              >
                <span slot="open">是</span>
                <span slot="close">否</span>
              </i-switch>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="14">
            <FormItem label="是否启用举报信箱" prop="isshowReport">
              <i-switch
                v-model="formModel.fields.isshowReport"
                :disabled="formModel.mode == 'show'"
              >
                <span slot="open">是</span>
                <span slot="close">否</span>
              </i-switch>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16">
          <Col span="14">
            <FormItem label="是否启用商户码" prop="isYard">
              <i-switch
                v-model="formModel.fields.isYard"
                :disabled="formModel.mode == 'show'"
              >
                <span slot="open">是</span>
                <span slot="close">否</span>
              </i-switch>
            </FormItem>
          </Col>
        </Row>
        <Row :gutter="16" v-show="formModel.fields.isYard">
          <FormItem label="商户码" prop="yard">
            <Input
              :readonly="formModel.mode == 'show'"
              v-model="formModel.fields.yard"
              placeholder="请输入商户码"
            />
          </FormItem>
        </Row>
        <Row :gutter="16" v-show="formModel.fields.isYard">
          <FormItem label="密钥" prop="secretkey">
            <Input
              type="password"
              :readonly="formModel.mode == 'show'"
              v-model="formModel.fields.secretkey"
              placeholder="请输入密钥"
            />
          </FormItem>
        </Row>
        <Row :gutter="16" >
          <FormItem label="微信小程序菜单显示(最多勾选7个)" prop="menus">
            <CheckboxGroup v-model="formModel.menus" v-for="(item,index) in checkedMenus" @on-change="checkChange">
              <Checkbox :label="item" :disabled="formModel.mode == 'show'"></Checkbox>
            </CheckboxGroup>
          </FormItem>
        </Row>
        <Row v-show="!formModel.fields.isYard">
          <FormItem label="二维码图片">
            <div class="demo-upload-list" v-for="item in auploadList">
              <template v-if="item.status === 'finished'">
                <img :src="item.url" />
                <div class="demo-upload-list-cover">
                  <Icon
                    type="ios-eye-outline"
                    style="float: left"
                    @click.native="ahandleViewFile(item.url)"
                  ></Icon>
                  <Icon
                    type="ios-trash-outline"
                    style="float: right"
                    @click.native="ahandleRemoveFile(item.name)"
                    v-show="!checkShow()"
                  ></Icon>
                </div>
              </template>
              <template v-else>
                <Progress
                  v-if="item.showProgress"
                  :percent="item.percentage"
                  hide-info
                ></Progress>
              </template>
            </div>
            <Divider dashed />
            <Upload
              ref="aupload"
              :show-upload-list="false"
              :default-file-list="adefaultList"
              :on-success="ashowUpResult"
              :on-progress="atoUpResult"
              :format="['jpg', 'jpeg', 'png']"
              :max-size="5120"
              :data="{ uuid: this.$store.state.user.userGuid }"
              :on-format-error="handleFormatError"
              :on-exceeded-size="handleMaxSize"
              :before-upload="handleBeforeUpload"
              type="drag"
              :action="actionurl"
              :headers="postheaders"
              style="display: inline-block; width: 58px"
              v-show="!checkShow()"
            >
              <div style="width: 58px; height: 58px; line-height: 58px">
                <Icon type="ios-camera" size="20"></Icon>
              </div>
            </Upload>
          </FormItem>
          <Modal title="查看图片" v-model="avisible">
            <img :src="aimgName" v-if="avisible" style="width: 100%" />
          </Modal>
        </Row>
        <Row
          :gutter="16"
          style="height: 320px"
          v-show="!formModel.fields.isYard"
        >
          <quill-editor
            @focus="onEditorFocus($event)"
            class="mec"
            v-model="formModel.fields.accessory"
          ></quill-editor>
        </Row>
        <!-- <Row :gutter="16">
          <Col span="14">
            <FormItem label="是否启用在线订餐" prop="isreservation">
              <i-switch v-model="formModel.fields.isreservation" :disabled="formModel.mode=='show'">
                <span slot="open">是</span>
                <span slot="close">否</span>
              </i-switch>
            </FormItem>
          </Col>
        </Row>-->
        <Row>
          <FormItem label="首页图片">
            <div class="demo-upload-list" v-for="item in uploadList">
              <template v-if="item.status === 'finished'">
                <img :src="item.url" />
                <div class="demo-upload-list-cover">
                  <Icon
                    type="ios-eye-outline"
                    style="float: left"
                    @click.native="handleViewFile(item.url)"
                  ></Icon>
                  <Icon
                    type="ios-trash-outline"
                    style="float: right"
                    @click.native="handleRemoveFile(item.name)"
                    v-show="!checkShow()"
                  ></Icon>
                </div>
              </template>
              <template v-else>
                <Progress
                  v-if="item.showProgress"
                  :percent="item.percentage"
                  hide-info
                ></Progress>
              </template>
            </div>
            <Divider dashed />
            <Upload
              ref="upload"
              :show-upload-list="false"
              :default-file-list="defaultList"
              :on-success="showUpResult"
              :on-progress="toUpResult"
              :format="['jpg', 'jpeg', 'png']"
              :max-size="5120"
              :data="{ uuid: this.$store.state.user.userGuid }"
              :on-format-error="handleFormatError"
              :on-exceeded-size="handleMaxSize"
              :before-upload="handleBeforeUpload"
              type="drag"
              :action="actionurl"
              :headers="postheaders"
              style="display: inline-block; width: 58px"
              v-show="!checkShow()"
            >
              <div style="width: 58px; height: 58px; line-height: 58px">
                <Icon type="ios-camera" size="20"></Icon>
              </div>
            </Upload>
          </FormItem>
          <Modal title="查看图片" v-model="visible">
            <img :src="imgName" v-if="visible" style="width: 100%" />
          </Modal>
        </Row>
        <!-- <Row v-if="checkShow()">
          <div class="demo-upload-list" v-for="item in uploadList">
            <template v-if="item.status === 'finished'">
              <img :src="item.url" />
              <div class="demo-upload-list-cover">
                <Icon type="ios-eye-outline" @click.native="handleViewFile(item.url)"></Icon>
              </div>
            </template>
            <template v-else>
              <Progress v-if="item.showProgress" :percent="item.percentage" hide-info></Progress>
            </template>
          </div>
        </Row> -->
      </Form>
      <div v-if="formModel.mode != 'show'" class="demo-drawer-footer">
        <Button
          icon="md-checkmark-circle"
          type="primary"
          @click="handleSubmitSchool"
          >保 存</Button
        >
        <Button
          style="margin-left: 8px"
          icon="md-close"
          @click="formModel.opened = false"
          >取 消</Button
        >
      </div>
    </Drawer>
  </div>
</template>

<script>
import DzTable from "_c/tables/dz-table.vue";
import {
  getSchoolList,
  createSchool,
  loadSchool,
  editSchool,
  deleteSchool,
  batchCommand,
  deleteFile,
  dataToSync,
  getWxMenu,
} from "@/api/rbac/school";
import {
  globalvalidateIDCardNoMust,
  globalvalidatepwd,
  globalvalidateIsNotEmpty,
} from "@/global/validate";
import config from "@/config";
import { getToken } from "@/libs/util";
export default {
  name: "school",
  components: {
    DzTable,
  },
  data() {
    return {
      loading: false,
      uploadList: [],
      loadingStatus: false,
      imgName: "",
      visible: false,
      defaultList: [],
      actionurl: "",
      postheaders: "",

      auploadList: [],
      aimgName: "",
      avisible: false,
      adefaultList: [],
      checkedMenus: [],
      isshowptjob: true,
      commands: {
        delete: { name: "delete", title: "删除" },
        recover: { name: "recover", title: "恢复" },
        // forbidden: { name: "forbidden", title: "禁用" },
        // normal: { name: "normal", title: "启用" }
      },
      formModel: {
        opened: false,
        title: "创建用户",
        mode: "create",
        selection: [],
        menus:[],
        fields: {
          schoolUuid: "",
          schoolName: "",
          schoolType: "",
          addTime: "",
          isDelete: 0,
          addPeople: "",
          purchasingStandard: "",
          link: "",
          yard: "",
          secretkey: "",
          schoolImg: "",
          isptjob: false,
          isrecharge: false,
          isreservation: false,
          isshowReport: true,
          isCuiPrices: true,
          isYard: true,
          qrcode: "",
          accessory: "",
          menus: "",
        },
        schoolType: "",
        cuisinetype: [
          { type: "幼儿园" },
          { type: "小学" },
          { type: "初中" },
          { type: "高中" },
        ],
        rules: {
          schoolName: [
            {
              validator: globalvalidateIsNotEmpty,
              type: "string",
              required: true,
              message: "请输入学校名称",
            },
          ],
        },
      },
      stores: {
        school: {
          query: {
            totalCount: 0,
            pageSize: 20,
            currentPage: 1,
            kw: "",
            isDeleted: 0,
            status: -1,
            sort: [
              {
                direct: "DESC",
                field: "ID",
              },
            ],
          },
          sources: {
            userTypes: [
              { value: 0, text: "超级管理员" },
              { value: 1, text: "管理员" },
              { value: 2, text: "普通用户" },
            ],
            isDeletedSources: [
              { value: -1, text: "全部" },
              { value: 0, text: "正常" },
              { value: 1, text: "已删" },
            ],
            statusSources: [
              { value: -1, text: "全部" },
              { value: 0, text: "禁用" },
              { value: 1, text: "正常" },
            ],
            statusFormSources: [
              { value: 0, text: "禁用" },
              { value: 1, text: "正常" },
            ],
            WxMenus: [],
          },
          columns: [
            { type: "selection", width: 50, key: "handle" },
            {
              title: "学校名称",
              key: "schoolName",
              sortable: true,
              minWidth: 300,
            },
            { title: "添加时间", key: "addTime", minWidth: 100 },
            { title: "添加人", key: "addPeople", minWidth: 120 },
            {
              title: "操作",
              align: "center",
              width: 150,
              className: "table-command-column",
              slot: "action",
            },
          ],
          data: [],
        },
      },
    };
  },
  computed: {
    formTitle() {
      if (this.formModel.mode === "create") {
        return "添加学校";
      }
      if (this.formModel.mode === "edit") {
        return "编辑学校";
      }
      if (this.formModel.mode === "show") {
        return "查看学校";
      }
      return "";
    },
    selectedRows() {
      return this.formModel.selection;
    },
    selectedRowsId() {
      return this.formModel.selection.map((x) => x.schoolUuid);
    },
  },
  methods: {
    loadSchoolList() {
      getSchoolList(this.stores.school.query).then((res) => {
        this.stores.school.data = res.data.data;
        this.stores.school.query.totalCount = res.data.totalCount;
      });
    },
    //类别选择
    changetype(e) {
      this.formModel.fields.schoolType = e;
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
      this.uploadList = [];
      this.auploadList = [];
      this.$refs.upload.fileList = [
        { name: "", status: "finished", showProgress: false },
      ];
      console.log(row);
      this.formModel.menus=[];
      this.handleLoadWxMenus();
      this.handleSwitchFormModeToEdit();
      this.handleResetFormSchool();
      this.doLoadSchool(row.schoolUuid);
    },
    handleShow(row) {
      this.uploadList = [];
      this.auploadList = [];
      this.formModel.menus=[];
      this.handleLoadWxMenus();
      this.handleSwitchFormModeToShow();
      this.handleResetFormSchool();
      this.doLoadSchool(row.schoolUuid);
    },
    handleSelect(selection, row) {},
    handleSelectionChange(selection) {
      this.formModel.selection = selection;
    },
    handleRefresh() {
      this.loadSchoolList();
    },
    handleShowCreateWindow() {
      this.uploadList = [];
      this.formModel.menus=[];
      this.$refs.upload.clearFiles();
      this.handleLoadWxMenus();
      this.handleSwitchFormModeToCreate();
      this.handleOpenFormWindow();
      this.handleResetFormSchool();
    },
    handleSubmitSchool() {
      let valid = this.validateSchoolForm();
      //console.log(valid);
      //console.log(this.formModel.fields);
      if (valid) {
        if (
          this.formModel.fields.schoolType == "" ||
          this.formModel.fields.schoolType == null
        ) {
          this.$Message.warning("请选择学校类别");
          return;
        }
        if (this.auploadList.length != 1 && !this.formModel.fields.isYard) {
          this.$Message.warning("请上传一张二维码图片");
          return;
        }
        if (this.formModel.mode === "create") {
          this.doCreateSchool();
        }
        if (this.formModel.mode === "edit") {
          this.doEditSchool();
        }
      }
    },
    handleResetFormSchool() {
      console.log(this.formModel.fields);
      this.formModel.fields.schoolName = "";
      this.formModel.fields.purchasingStandard = "";
      //this.$refs["formSchool"].resetFields();
    },
    doCreateSchool() {
      this.formModel.fields.addPeople = this.$store.state.user.userName;
      this.formModel.fields.menus=this.formModel.menus.join(",");
      createSchool(this.formModel.fields).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.handleCloseFormWindow();
          this.loadSchoolList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    doEditSchool() {
      this.formModel.fields.addPeople = this.$store.state.user.userName;
      this.formModel.fields.menus=this.formModel.menus.join(",");
      editSchool(this.formModel.fields).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.handleCloseFormWindow();
          this.loadSchoolList();
        } else {
          this.$Message.warning(res.data.message);
        }
      });
    },
    validateSchoolForm() {
      let _valid = false;
      this.$refs["formSchool"].validate((valid) => {
        if (!valid) {
          this.$Message.error("请完善表单信息");
        } else {
          _valid = true;
        }
      });
      return _valid;
    },
    doLoadSchool(schoolUuid) {
      loadSchool({ guid: schoolUuid }).then((res) => {
        console.log(55555);
        console.log(res.data.data);
        this.formModel.fields = res.data.data;
        this.formModel.schoolType = res.data.data.schoolType;
        if (this.formModel.fields.schoolImg == null) {
          this.formModel.fields.schoolImg == "";
        }
        this.uploadList = [];
        if (this.formModel.fields.schoolImg) {
          let list = this.formModel.fields.schoolImg.split("|");

          for (let i = 0; i < list.length; i++) {
            this.uploadList.push({
              url: config.baseUrl.dev + "UploadFiles/SchoolImg/" + list[i],
              status: "finished",
              name: "UploadFiles/SchoolImg/" + list[i],
              fileName: list[i],
            });
          }
        }
        if (this.formModel.fields.qrcode) {
          let list = this.formModel.fields.qrcode.split("|");

          for (let i = 0; i < list.length; i++) {
            this.auploadList.push({
              url: config.baseUrl.dev + "UploadFiles/SchoolImg/" + list[i],
              status: "finished",
              name: "UploadFiles/SchoolImg/" + list[i],
              fileName: list[i],
            });
          }
        }
        this.formModel.menus=res.data.data.menus.split(",");

      });
    },
    handleDelete(row) {
      this.doDelete(row.schoolUuid);
    },
    doDelete(ids) {
      if (!ids) {
        this.$Message.warning("请选择至少一条数据");
        return;
      }
      console.log(1);
      deleteSchool(ids).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadSchoolList();
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
        },
      });
    },
    doBatchCommand(command) {
      batchCommand({
        command: command,
        ids: this.selectedRowsId.join(","),
      }).then((res) => {
        if (res.data.code === 200) {
          this.$Message.success(res.data.message);
          this.loadSchoolList();
          this.formModel.selection = [];
        } else {
          this.$Message.warning(res.data.message);
        }
        this.$Modal.remove();
      });
    },
    handleSearchSchool() {
      console.log(this.stores.school.query.isDeleted);
      this.loadSchoolList();
    },
    rowClsRender(row, index) {
      if (row.isDelete) {
        return "table-row-disabled";
      }
      return "";
    },
    handleSortChange(column) {
      this.stores.school.query.sort.direction = column.order;
      this.stores.school.query.sort.field = column.key;
      this.loadSchoolList();
    },
    handlePageChanged(page) {
      this.stores.school.query.currentPage = page;
      this.loadSchoolList();
    },
    handlePageSizeChanged(pageSize) {
      this.stores.school.query.pageSize = pageSize;
      this.loadSchoolList();
    },

    renderSchoolType(userType) {
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
      let _status = {
        color: "success",
        text: "正常",
      };
      switch (status) {
        case 0:
          _status.text = "禁用";
          _status.color = "error";
          break;
      }
      return _status;
    },
    checkShow() {
      return this.formModel.mode === "show";
    },
    async showUpResult(response, file, fileList) {
      console.log(this.$refs.upload.fileList);
      console.log(1);
      console.log(response);
      console.log(file);
      console.log(fileList);
      // this.loadingStatus = false;
      // this.updisabled = false;
      if (response.code == "200") {
        this.$Message.success(response.message);
        if (this.formModel.fields.schoolImg == null) {
          this.formModel.fields.schoolImg = "";
        }
        if (this.formModel.fields.schoolImg.length > 0) {
          this.formModel.fields.schoolImg += "|";
        }
        this.formModel.fields.schoolImg += response.data.fname;
        // this.formModel.dFileName = response.data.paths;

        await this.uploadList.push({
          url: config.baseUrl.dev + response.data.strpath.replace("\\", "/"),
          status: "finished",
          name: response.data.strpath,
          fileName: response.data.fname,
        });

        // console.log(
        //   (this.$refs.upload.fileList[0].name = e.data.dataPath.split("\\")[1])
        // );
        // console.log(this.defaultfilelist);
        // if (this.departmentlist.length >= 1) {
        //   this.defaultfilelist = [
        //     { name: this.formModel.fields.name, url: e.data.path }
        //   ];
        // }
        console.log(this.formModel.dFileName);
      } else {
        this.$Message.warning(response.message);
      }
    },
    async ashowUpResult(response, file, fileList) {
      if (response.code == "200") {
        this.$Message.success(response.message);
        if (this.formModel.fields.qrcode == null) {
          this.formModel.fields.qrcode = "";
        }
        if (this.formModel.fields.qrcode.length > 0) {
          this.formModel.fields.qrcode += "|";
        }
        this.formModel.fields.qrcode += response.data.fname;
        // this.formModel.dFileName = response.data.paths;

        await this.auploadList.push({
          url: config.baseUrl.dev + response.data.strpath.replace("\\", "/"),
          status: "finished",
          name: response.data.strpath,
          fileName: response.data.fname,
        });
        console.log(this.formModel.dFileName);
      } else {
        this.$Message.warning(response.message);
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
    },
    atoUpResult() {
      if (this.$refs.upload.fileList.length > 1) {
        this.$refs.upload.fileList.shift();
      }
    },
    handleFormatError(file) {
      this.$Notice.warning({
        title: "文件格式有误",
        desc: "文件: " + file.name + " 不是png,jpg",
      });
    },
    handleMaxSize(file) {
      this.$Notice.warning({
        title: "文件大小超限",
        desc: "文件 " + file.name + " 太大,超过5M.",
      });
    },
    handleBeforeUpload() {
      return true;
    },
    handleViewFile(name) {
      this.imgName = name;
      this.visible = true;
    },
    ahandleViewFile(name) {
      this.aimgName = name;
      this.avisible = true;
    },
    handleRemoveFile(file) {
      console.log(file);
      // const fileList = this.$refs.upload.fileList;
      // this.$refs.upload.fileList.splice(fileList.indexOf(file), 1);
      deleteFile({ path: file }).then((res) => {
        console.log(res);
        if (res.data.data == "200") {
          this.uploadList = this.uploadList.filter((x) => x.name != file);
          this.formModel.fields.schoolImg = this.uploadList
            .map((x) => x.fileName)
            .join("|");
        } else {
          this.uploadList = this.uploadList.filter((x) => x.name != file);
          this.formModel.fields.schoolImg = this.uploadList
            .map((x) => x.fileName)
            .join("|");
        }
      });
    },
    ahandleRemoveFile(file) {
      console.log(file);
      // const fileList = this.$refs.upload.fileList;
      // this.$refs.upload.fileList.splice(fileList.indexOf(file), 1);
      deleteFile({ path: file }).then((res) => {
        console.log(res);
        if (res.data.data == "200") {
          this.auploadList = this.auploadList.filter((x) => x.name != file);
          this.formModel.fields.qrcode = this.auploadList
            .map((x) => x.fileName)
            .join("|");
        } else {
          this.auploadList = this.auploadList.filter((x) => x.name != file);
          this.formModel.fields.qrcode = this.auploadList
            .map((x) => x.fileName)
            .join("|");
        }
      });
    },
    toSync() {
      this.loading = true;
      dataToSync().then((res) => {
        console.log(res);
        this.loading = false;
      });
    },
    onEditorFocus(event) {
      if (this.formModel.mode === "show") {
        event.enable(false);
      } else {
        event.enable(true);
      }
    }, // 获得焦点事件
    handleLoadWxMenus() {
      getWxMenu().then((res) => {
        console.log(res);
        if (res.data.code == 200) {
          this.checkedMenus=[];
          this.stores.school.sources.WxMenus = res.data.data;
          let list=this.stores.school.sources.WxMenus
          for(let i=0;i<list.length;i++){
            if(i==0){
              continue;
            }
            this.checkedMenus.push(list[i].menuName);
          }
        }
      });
    },
    checkChange(e){
      console.log(e);
      if(e.length>7){
        this.formModel.menus.pop();
      }
    }

  },
  mounted() {
    this.loadSchoolList();
    console.log(111111);
    console.log(this.$store.state);
    let menu = this.$store.state.app.menuList.find(
      (x) => x.meta.title == "勤工俭学"
    );
    console.log(menu);
    if (typeof menu == "undefined" || menu.meta.hideInMenu == true) {
      this.isshowptjob = false;
      console.log(this.isshowptjob);
    }
    // this.doloadRoleList();
    //this.doloadDepartmentListdetail();
  },
  created() {
    this.postheaders = {
      Authorization: "Bearer " + getToken(),
      //Accept: "application/json, text/plain, */*"
    };
    this.actionurl = config.baseUrl.dev + "api/v1/rbac/school/UpLoad";
  },
};
</script>
<style>
.ivu-checkbox-group{
  float: left;
}
.demo-upload-list {
  display: inline-block;
  width: 120px;
  height: 120px;
  text-align: center;
  line-height: 120px;
  border: 1px solid transparent;
  border-radius: 4px;
  overflow: hidden;
  background: #fff;
  position: relative;
  box-shadow: 0 1px 1px rgba(0, 0, 0, 0.2);
  margin-right: 4px;
}
.demo-upload-list img {
  width: 100%;
  height: 100%;
}
.demo-upload-list-cover {
  display: none;
  position: absolute;
  top: 0;
  bottom: 0;
  left: 0;
  right: 0;
  background: rgba(0, 0, 0, 0.6);
}
.demo-upload-list:hover .demo-upload-list-cover {
  display: block;
}
.demo-upload-list-cover i {
  color: #fff;
  font-size: 20px;
  cursor: pointer;
  margin: 0 2px;
}
.mec {
  height: 200px;
}
</style>
